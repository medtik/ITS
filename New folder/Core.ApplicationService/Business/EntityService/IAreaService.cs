namespace Core.ApplicationService.Business.EntityService
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Core.ObjectModels.Entities;

    public interface IAreaService
    {
        IQueryable<Area> Search(Expression<Func<Area, bool>> searchValue, params Expression<Func<Area, object>>[] includes);
    }
}