namespace Service.Implement.Entity
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Core.ObjectModels.Entities;
    using Core.ObjectService.Repositories;
    using Core.ApplicationService.Business.EntityService;
    using Core.ApplicationService.Business.LogService;
    using System.Collections.Generic;
    using System.Transactions;

    public class QuestionService : _BaseService<Question>, IQuestionService
    {
        private readonly IRepository<Answer> _answerRepository;

        public QuestionService(ILoggingService loggingService, IUnitOfWork unitOfWork) : base(loggingService, unitOfWork)
        {
            _answerRepository = unitOfWork.GetRepository<Answer>();
        }

        public bool Create(Question question, IEnumerable<Answer> answers)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    base.Create(question);
                    foreach (var answer in answers)
                    {
                        answer.QuestionId = question.Id;
                        _answerRepository.Create(answer);
                    }
                    _unitOfWork.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Create), ex);
                return false;
            }
        }

        public bool Delete(int questionId)
        {
            Question entity = Find(questionId);
            return entity == null ? false : base.Delete(entity);
        }

        public Question Find(int questionId, params Expression<Func<Question, object>>[] includes)
        {
            Question question = null;
            try
            {
                question = _repository.GetAsQueryable(_ => _.Id == questionId, includes);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Find), ex);
            }

            return question;
        }

        public IQueryable<Question> GetAll(params Expression<Func<Question, object>>[] includes)
        {
            IQueryable<Question> questions = null;
            try
            {
                questions = _repository.GetAllAsQueryable(includes);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(GetAll), ex);
            }

            return questions ?? Enumerable.Empty<Question>().AsQueryable();
        }

        public IQueryable<Answer> GetAnswerByQuestion(int questionId, params Expression<Func<Answer, object>>[] includes)
        {
            IQueryable<Answer> answers = null;
            try
            {
                answers = _answerRepository.SearchAsQueryable(_ => _.QuestionId == questionId, includes);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(GetAll), ex);
            }

            return answers ?? Enumerable.Empty<Answer>().AsQueryable();
        }

        public IEnumerable<string> GetCategories()
        {
            return GetAll().Select(_ => _.Categories);
        }

        public IQueryable<Question> GetQuestionByArea(int areaId, params Expression<Func<Question, object>>[] includes)
        {
            IQueryable<Question> questions = null;
            try
            {
                questions = _repository.SearchAsQueryable(_ => _.Areas.Any(__ => __.Id == areaId), includes);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(GetAll), ex);
            }

            return questions ?? Enumerable.Empty<Question>().AsQueryable();
        }

        public IQueryable<Question> Search(Expression<Func<Question, bool>> searchValue, params Expression<Func<Question, object>>[] includes)
        {
            IQueryable<Question> questions = null;
            try
            {
                questions = _repository.SearchAsQueryable(searchValue, includes);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Search), ex);
            }

            return questions ?? Enumerable.Empty<Question>().AsQueryable();
        }
    }
}