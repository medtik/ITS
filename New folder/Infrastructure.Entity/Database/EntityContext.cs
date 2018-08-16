namespace Infrastructure.Entity.Database
{
    using Core.ApplicationService.Database.Entities;

    public class EntityContext : IEntityContext
    {
        public object GetContext => new ITSContext();
    }
}