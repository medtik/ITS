namespace API.ITSProject_2.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Newtonsoft.Json;
    using Core.ObjectModels.Entities;
    using Core.ObjectModels.Pagination;
    using Core.ApplicationService.Business.EntityService;
    using Core.ApplicationService.Business.IdentityService;
    using Core.ApplicationService.Business.LogService;
    using Core.ApplicationService.Business.PagingService;
    using API.ITSProject_2.ViewModels;

    public class QuestionController : _BaseController
    {
        private readonly IQuestionService _questionService;
        private readonly IAreaService _areaService;
        private readonly ITagService _tagService;

        public QuestionController(ILoggingService loggingService, IPagingService paggingService, IAreaService areaService,
            IIdentityService identityService, IQuestionService questionService, ITagService tagService, IPhotoService photoService) : base(loggingService, paggingService, identityService, photoService)
        {
            this._questionService = questionService;
            this._tagService = tagService;
            _areaService = areaService;
        }

        #region Get
        [HttpGet]
        [Route("api/Question/Categories")]
        public IHttpActionResult GetCategories()
        {
            return Ok(_questionService.GetCategories());
        }

        [HttpGet]
        [Route("api/Question/Detail")]
        public IHttpActionResult GetQuestionDetails(int id)
        {
            try
            {
                var quest = _questionService.Find(id, _ => _.Answers.Select(__ => __.Tags));
                
                return Ok(ModelBuilder.ConvertToQuestionDetailsViewModels(quest));
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(GetQuestionDetails), ex);

                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Question/QuestionSearch")]
        public IHttpActionResult QuestionSearch(string content)
        {
            return Ok(ModelBuilder.ConvertToQuestionDetailsViewModels(_questionService.Search(_ => _.Content.Contains(content), _ => _.Answers.Select(__ => __.Tags))));
        }

        [HttpGet]
        [Route("api/Question/QuestionsByArea")]
        public IHttpActionResult GetQuestionByArea(int areaId)
        {
            return Ok(ModelBuilder.ConvertToQuestionDetailsViewModels(_questionService.GetQuestionByArea(areaId, _ => _.Answers.Select(__ => __.Tags))));
        }

        [HttpGet]
        public IHttpActionResult Get(string searchValue = "", string orderBy = "", int? pageIndex = 1, int? pageSize = 10)
        {
            IEnumerable<QuestionViewModels> currentList = Enumerable.Empty<QuestionViewModels>();
            try
            {
                Pager<Question> pager = null;
                IQueryable<Question> listQuestions = _questionService.Search(_ => string.IsNullOrEmpty(searchValue) ||
                                                        _.Content.Contains(searchValue) ||
                                                        _.Categories.Contains(searchValue), _ => _.Answers);

                orderBy = orderBy?.ToLower();
                switch (orderBy)
                {
                    case "answercount_asc":
                        pager = _paggingService.ToPagedList(listQuestions.
                            OrderBy(e => e.Answers.Count), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "answercount_desc":
                        pager = _paggingService.ToPagedList(listQuestions.
                            OrderByDescending(e => e.Answers.Count), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "categories_asc":
                        pager = _paggingService.ToPagedList(listQuestions.
                            OrderBy(e => e.Categories), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "categories_desc":
                        pager = _paggingService.ToPagedList(listQuestions.
                            OrderByDescending(e => e.Categories), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "content_asc":
                        pager = _paggingService.ToPagedList(listQuestions.
                            OrderBy(e => e.Content), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    default:
                        //"content_desc"
                        pager = _paggingService.ToPagedList(listQuestions.
                            OrderByDescending(e => e.Content), pageIndex ?? 1, pageSize ?? 10);
                        break;
                }

                currentList = ModelBuilder.ConvertToViewModels(pager.CurrentList);

                CurrentContext.Response.Headers.Add("Paging-Header", JsonConvert.SerializeObject(new
                {
                    pageIndex = pager.PageIndex,
                    pageSize = pager.PageSize,
                    totalElement = pager.TotalElement,
                    totalPage = pager.TotalPage,
                    orderBy,
                    searchValue
                }));//add meta-data to current response's header

                return Ok(new
                {
                    meta = new
                    {
                        pageIndex = pager.PageIndex,
                        pageSize = pager.PageSize,
                        totalElement = pager.TotalElement,
                        totalPage = pager.TotalPage,
                        orderBy,
                        searchValue
                    },
                    currentList
                });
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Get), ex);

                return InternalServerError(ex);
            }
        }
        #endregion

        #region Post
        [HttpPost]
        public IHttpActionResult Post(CreateQuestionViewModels data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Question question = ModelBuilder.ConvertToModels(data);
                    ICollection<Answer> answers = new List<Answer>();

                    if (data.Answers != null)
                    {
                        data.Answers.ToList().ForEach(_ => {
                            ICollection<Tag> tags = new List<Tag>();
                            _.Tags.ToList().ForEach(__ =>
                            {
                                tags.Add(_tagService.Find(__));
                            });

                            answers.Add(new Answer
                            {
                                Content = _.Answer,
                                Tags = tags
                            });
                        });
                    }
                    _questionService.Create(question, answers);

                    return Ok();
                } else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Post), ex);

                return InternalServerError(ex);
            }
        }
        #endregion

        #region Delete
        [HttpDelete]
        public IHttpActionResult Delete(int questionId)
        {
            try
            {
                _questionService.Delete(questionId);

                return Ok();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Delete), ex);

                return InternalServerError(ex);
            }
        }
        #endregion

        #region Put
        [HttpPut]
        public IHttpActionResult EditQuestion(CreateQuestionViewModels data,[FromBody] int id)
        {

            if (ModelState.IsValid)
            {
                var baseQuestion = _questionService.Find(id, _ => _.Areas);

                var area = baseQuestion.Areas.Select(_ => _areaService.Find(_.Id, __ => __.Questions));
                Question question = ModelBuilder.ConvertToModels(data);
                ICollection<Answer> answers = new List<Answer>();

                if (data.Answers != null)
                {
                    data.Answers.ToList().ForEach(_ => {
                        ICollection<Tag> tags = new List<Tag>();
                        _.Tags.ToList().ForEach(__ =>
                        {
                            tags.Add(_tagService.Find(__));
                        });

                        answers.Add(new Answer
                        {
                            Content = _.Answer,
                            Tags = tags
                        });
                    });
                }
                _questionService.Create(question, answers);
                foreach (var item in area)
                {
                    item.Questions.Add(question);
                    _areaService.Update(item);
                }
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion
    }
}