namespace API.ITSProject.Controllers
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
    using API.ITSProject.ViewModels;

    public class TagController : _BaseController
    {
        private readonly ITagService _tagService;

        public TagController(ILoggingService loggingService, IPagingService paggingService, 
            IIdentityService identityService, ITagService tagService) : base(loggingService, paggingService, identityService)
        {
            this._tagService = tagService;
        }

        [HttpGet]
        [Route("api/Tag/categories")]
        public IHttpActionResult GetCategories()
        {
            return Ok(_tagService.GetCategories());
        }

        [HttpGet]
        public IHttpActionResult Get(string searchValue = "", string orderBy = "", int? pageIndex = 1, int? pageSize = 10)
        {
            IEnumerable<TagViewModels> currentList = Enumerable.Empty<TagViewModels>();
            try
            {
                Pager<Tag> pager = null;
                IQueryable<Tag> listEmployees = _tagService.Search(_ => string.IsNullOrEmpty(searchValue) || _.Name.Contains(searchValue) || 
                                                    _.Categories.Contains(searchValue), _ => _.Locations);

                orderBy = orderBy?.ToLower();
                switch (orderBy)
                {
                    case "locationcount_asc":
                        pager = _paggingService.ToPagedList(listEmployees.
                            OrderBy(e => e.Locations.Count), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "locationcount_desc":
                        pager = _paggingService.ToPagedList(listEmployees.
                            OrderByDescending(e => e.Locations.Count), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "categories_asc":
                        pager = _paggingService.ToPagedList(listEmployees.
                            OrderBy(e => e.Categories), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "categories_desc":
                        pager = _paggingService.ToPagedList(listEmployees.
                            OrderByDescending(e => e.Categories), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "name_asc":
                        pager = _paggingService.ToPagedList(listEmployees.
                            OrderBy(e => e.Name), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    default:
                        //"name_desc"
                        pager = _paggingService.ToPagedList(listEmployees.
                            OrderByDescending(e => e.Name), pageIndex ?? 1, pageSize ?? 10);
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

        [HttpPost]
        public IHttpActionResult Post(CreateTagViewModels data)
        {
            try
            {
                Tag tag = ModelBuilder.ConvertToModels(data);

                _tagService.Create(tag);
                return Ok();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Post), ex);

                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(int tagId)
        {
            try
            {
                _tagService.Delete(tagId);

                return Ok();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Delete), ex);

                return InternalServerError(ex);
            }
        }
    }
}
