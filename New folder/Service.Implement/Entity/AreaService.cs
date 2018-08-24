namespace Service.Implement.Entity
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Core.ObjectModels.Entities;
    using Core.ObjectService.Repositories;
    using Core.ApplicationService.Business.EntityService;
    using Core.ApplicationService.Business.LogService;

    public class AreaService : _BaseService<Area>, IAreaService
    {
        public AreaService(ILoggingService loggingService, IUnitOfWork unitOfWork) : base(loggingService, unitOfWork)
        {
        }

        public Area Find(int id, params Expression<Func<Area, object>>[] includes)
            => _repository.Get(_ => _.Id == id, includes);

        public IQueryable<Area> Search(Expression<Func<Area, bool>> searchValue, params Expression<Func<Area, object>>[] includes)
        {
            IQueryable<Area> areas = null;
            try
            {
                areas = _repository.SearchAsQueryable(searchValue, includes);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Search), ex);
            }

            return areas ?? Enumerable.Empty<Area>().AsQueryable();
        }
    }
}