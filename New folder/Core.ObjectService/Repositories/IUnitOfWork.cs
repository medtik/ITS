namespace Core.ObjectService.Repositories
{
    public interface IUnitOfWork
    {
        void SaveChanges();

        IRepository<T> GetRepository<T>() where T : class;
    }
}
