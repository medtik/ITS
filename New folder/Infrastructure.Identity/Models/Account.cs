namespace Infrastructure.Identity.Models
{
    using Microsoft.AspNet.Identity.EntityFramework;

    public class Account : IdentityUser
    {
        public int UserId { get; set; }
    }
}