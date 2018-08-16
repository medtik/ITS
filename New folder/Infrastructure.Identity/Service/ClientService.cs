namespace Infrastructure.Identity.Service
{
    using System.Data.Entity;
    using Core.ObjectModels.Identity;
    using Core.ApplicationService.Database.Identities;

    public class ClientService
    {
        private DbContext _dbContext;

        public ClientService(IIdentityContext context)
        {
            _dbContext = context.GetContext as DbContext;
        }

        public Client FindClient(string clientId)
        {
            var client = _dbContext.Set<Client>().Find(clientId);

            return client;
        }
    }
}