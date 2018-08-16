namespace Core.ApplicationService.Business.EntityService
{
    using System;
    using System.Linq.Expressions;
    using Core.ObjectModels.Entities;

    public interface IAnswerService
    {
        bool Create(Answer answer);

        bool Delete(int answerId);

        Answer Find(int answerId, params Expression<Func<Answer, object>>[] includes);
    }
}