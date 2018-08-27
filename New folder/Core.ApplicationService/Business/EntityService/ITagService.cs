namespace Core.ApplicationService.Business.EntityService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Core.ObjectModels.Entities;

    public interface ITagService
    {
        bool Create(Tag tag);

        bool Delete(int tagId);

        bool Update(Tag tag);

        IEnumerable<string> GetCategories();

        Tag Find(int tagId, params Expression<Func<Tag, object>>[] includes);

        IQueryable<Tag> GetAll(params Expression<Func<Tag, object>>[] includes);

        IQueryable<Tag> Search(Expression<Func<Tag, bool>> searchValue, params Expression<Func<Tag, object>>[] includes);
    }
}