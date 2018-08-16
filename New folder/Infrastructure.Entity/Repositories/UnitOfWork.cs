namespace Infrastructure.Entity.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using Core.ObjectService.Repositories;
    using Core.ApplicationService.Database.Entities;

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool _isDisposed;
        private DbContext _dbContext;
        private IDictionary<Type, object> _repositories;

        public UnitOfWork(IEntityContext entityContext)
        {
            _dbContext = entityContext.GetContext as DbContext;
            if (_dbContext == null)
            {
                throw new ArgumentException();
            }//end if check is the context validation

            _repositories = new Dictionary<Type, object>();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            IRepository<T> repository;
            if (_repositories.Keys.Contains(typeof(T)))
            {
                repository = _repositories[typeof(T)] as IRepository<T>;
            }
            else
            {
                repository = new Repository<T>(_dbContext);
                this._repositories.Add(typeof(T), repository);
            }//end if using singleton theory

            return repository;
        }

        public void SaveChanges() => _dbContext.SaveChanges();

        #region Implement disposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this._isDisposed)
            {
                if (disposing)
                {
                    this._dbContext.Dispose();
                }
            }
            this._isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}