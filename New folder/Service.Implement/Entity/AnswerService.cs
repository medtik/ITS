namespace Service.Implement.Entity
{
    using System;
    using System.Linq.Expressions;
    using Core.ObjectModels.Entities;
    using Core.ObjectService.Repositories;
    using Core.ApplicationService.Business.LogService;
    using Core.ApplicationService.Business.EntityService;

    public class AnswerService : _BaseService<Answer>, IAnswerService
    {
        public AnswerService(ILoggingService loggingService, IUnitOfWork unitOfWork) : base(loggingService, unitOfWork)
        {
        }

        public bool Delete(int answerId)
        {
            Answer entity = Find(answerId);
            return entity == null ? false : base.Delete(entity);
        }

        public Answer Find(int answerId, params Expression<Func<Answer, object>>[] includes)
        {
            Answer answer = null;
            try
            {
                answer = _repository.GetAsQueryable(_ => _.Id == answerId, includes);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Find), ex);
            }

            return answer;
        }
    }
}