using SharpRaven;

namespace Service.Implement.Entity
{
    using System;
    using System.Transactions;
    using Core.ObjectService.Repositories;
    using Core.ApplicationService.Business.LogService;

    public abstract class _BaseService<T> where T : class
    {
        protected readonly ILoggingService _loggingService;

        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IRepository<T> _repository;


        public _BaseService(ILoggingService loggingService, IUnitOfWork unitOfWork)
        {
            _loggingService = loggingService;

            _unitOfWork = unitOfWork;
            _repository = unitOfWork.GetRepository<T>();
        }

        public virtual bool Create(T entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    _repository.Create(entity);
                    _unitOfWork.SaveChanges();

                    scope.Complete();
                    return true;
                }//end scope
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Create), ex);
                return false;
            }
        }

        public virtual bool Update(T entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    _repository.Update(entity);
                    _unitOfWork.SaveChanges();

                    scope.Complete();
                    return true;
                }//end scope
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Update), ex);
                return false;
            }
        }

        public virtual bool Delete(T entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    _repository.Delete(entity);
                    _unitOfWork.SaveChanges();

                    scope.Complete();
                    return true;
                }//end scope
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Delete), ex);
                return false;
            }
        }
    }
}