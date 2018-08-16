namespace Service.Implement.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Core.ApplicationService.Business.EntityService;
    using Core.ApplicationService.Business.LogService;
    using Core.ObjectModels.Entities;
    using Core.ObjectService.Repositories;

    public class TagService : _BaseService<Tag>, ITagService
    {
        public TagService(ILoggingService loggingService, IUnitOfWork unitOfWork) : base(loggingService, unitOfWork)
        {
        }

        public bool Delete(int tagId)
        {
            Tag entity = Find(tagId);
            return entity == null ? false : base.Delete(entity);
        }

        public Tag Find(int tagId, params Expression<Func<Tag, object>>[] includes)
        {
            Tag tag = null;
            try
            {
                tag = _repository.GetAsQueryable(_ => _.Id == tagId, includes);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Find), ex);
            }

            return tag;
        }

        public IQueryable<Tag> GetAll(params Expression<Func<Tag, object>>[] includes)
        {
            IQueryable<Tag> tags = null;
            try
            {
                tags = _repository.GetAllAsQueryable(includes);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(GetAll), ex);
            }

            return tags ?? Enumerable.Empty<Tag>().AsQueryable();
        }

        public IEnumerable<string> GetCategories()
        {
            return GetAll().Select(_ => _.Categories);
        }

        public IQueryable<Tag> Search(Expression<Func<Tag, bool>> searchValue, params Expression<Func<Tag, object>>[] includes)
        {
            IQueryable<Tag> tags = null;
            try
            {
                tags = _repository.SearchAsQueryable(searchValue, includes);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Search), ex);
            }

            return tags ?? Enumerable.Empty<Tag>().AsQueryable();
        }
    }
}