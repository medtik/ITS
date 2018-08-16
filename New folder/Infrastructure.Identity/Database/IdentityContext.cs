using Core.ApplicationService.Database.Identities;

namespace Infrastructure.Identity.Database
{
    public class IdentityContext : IIdentityContext
    {
        public object GetContext => new IdentityITSContext();
    }
}