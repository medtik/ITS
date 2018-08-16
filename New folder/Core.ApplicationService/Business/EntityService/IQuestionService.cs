namespace Core.ApplicationService.Business.EntityService
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;
    using Core.ObjectModels.Entities;

    public interface IQuestionService
    {
        bool Create(Question question, IEnumerable<Answer> answer);

        bool Delete(int questionId);

        IEnumerable<string> GetCategories();

        Question Find(int questionId, params Expression<Func<Question, object>>[] includes);

        IQueryable<Question> GetAll(params Expression<Func<Question, object>>[] includes);

        IQueryable<Question> Search(Expression<Func<Question, bool>> searchValue, params Expression<Func<Question, object>>[] includes);

        IQueryable<Question> GetQuestionByArea(int areaId, params Expression<Func<Question, object>>[] includes);

        IQueryable<Answer> GetAnswerByQuestion(int questionId, params Expression<Func<Answer, object>>[] includes);
    }
}