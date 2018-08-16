namespace Core.ApplicationService.Database
{
    public interface IDbContext
    {
        object GetContext { get; }
    }
}