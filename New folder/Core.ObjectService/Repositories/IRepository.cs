namespace Core.ObjectService.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    public interface IRepository<T> where T : class
    {
        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        #region Filter by database
        T GetAsQueryable(Expression<Func<T, bool>> conditions, params Expression<Func<T, object>>[] includes);

        IQueryable<T> GetAllAsQueryable(params Expression<Func<T, object>>[] includes);

        IQueryable<T> SearchAsQueryable(Expression<Func<T, bool>> conditions, params Expression<Func<T, object>>[] includes);

        IQueryable<TEntity> SelectAsQueryable<TEntity>(Expression<Func<T, TEntity>> selector, params Expression<Func<T, object>>[] includes);
        #endregion

        #region Filter by memory
        T Get(Func<T, bool> conditions, params Expression<Func<T, object>>[] includes);

        IEnumerable<T> Search(Func<T, bool> conditions, params Expression<Func<T, object>>[] includes);

        IEnumerable<TEntity> Select<TEntity>(Func<T, TEntity> selector, params Expression<Func<T, object>>[] includes);
        #endregion
    }
}