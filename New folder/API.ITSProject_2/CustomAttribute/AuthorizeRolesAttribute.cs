namespace API.ITSProject_2.CustomAttribute
{
    using System.Web.Http;

    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles) : base() => Roles = string.Join(",", roles);
    }
}