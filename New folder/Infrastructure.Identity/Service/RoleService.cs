namespace Infrastructure.Identity.Service
{
    using Microsoft.AspNet.Identity;
    using Infrastructure.Identity.Models;

    public class RoleService : RoleManager<Role>
    {
        public RoleService(IRoleStore<Role, string> store) : base(store)
        {
        }
    }
}