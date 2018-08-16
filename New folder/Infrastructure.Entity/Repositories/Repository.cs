namespace Infrastructure.Entity.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Data.Entity;
    using System.Collections.Generic;
    using Core.ObjectService.Repositories;

    public class Repository<T> : IRepository<T> where T : class
    {
        public DbContext _dbContext { get; set; }

        public DbSet<T> _dbSet { get; set; }

        public Repository(DbContext dbContext)
        {
            this._dbContext = dbContext;
            this._dbSet = dbContext.Set<T>();
        }

        public void Create(T entity) => _dbSet.Add(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public T GetAsQueryable(Expression<Func<T, bool>> conditions, params Expression<Func<T, object>>[] includes)
            => GetAllAsQueryable(includes).FirstOrDefault(conditions);

        public T Get(Func<T, bool> conditions, params Expression<Func<T, object>>[] includes)
            => GetAllAsQueryable(includes).FirstOrDefault(conditions);

        public IQueryable<T> GetAllAsQueryable(params Expression<Func<T, object>>[] includes)
        {
            DbSet<T> dbSet = this._dbSet;
            IQueryable<T> result = dbSet;

            foreach (Expression<Func<T, object>> include in includes)
            {
                result = result.Include(include);
            }//end for include all related object

            return result;
        }

        public IQueryable<T> SearchAsQueryable(Expression<Func<T, bool>> conditions, params Expression<Func<T, object>>[] includes)
            => GetAllAsQueryable(includes).Where(conditions);

        public IEnumerable<T> Search(Func<T, bool> conditions, params Expression<Func<T, object>>[] includes)
            => GetAllAsQueryable(includes).Where(conditions);

        public IQueryable<TEntity> SelectAsQueryable<TEntity>(Expression<Func<T, TEntity>> selector, params Expression<Func<T, object>>[] includes)
            => GetAllAsQueryable(includes).Select(selector);

        public IEnumerable<TEntity> Select<TEntity>(Func<T, TEntity> selector, params Expression<Func<T, object>>[] includes)
            => GetAllAsQueryable(includes).Select(selector);

        public void Update(T entity)
        {
            if (this._dbContext.Entry<T>(entity).State == EntityState.Detached)
            {
                this._dbSet.Attach(entity);
            }//end if check is the entity attach

            this._dbContext.Entry<T>(entity).State = EntityState.Modified;
        }
    }
}
