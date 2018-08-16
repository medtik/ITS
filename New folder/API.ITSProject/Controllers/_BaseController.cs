namespace API.ITSProject.Controllers
{
    using System.Web;
    using System.Web.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Core.ObjectModels.Entities;
    using Core.ApplicationService.Business.LogService;
    using Core.ApplicationService.Business.PagingService;
    using Core.ApplicationService.Business.IdentityService;
    using API.ITSProject.ViewModels;
    using Microsoft.Owin.Security;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Text;

    public abstract class _BaseController : ApiController
    {
        private _ModelBuilder _modelBuilder;

        protected readonly ILoggingService _loggingService;
        protected readonly IPagingService _paggingService;

        protected readonly IIdentityService _identityService;

        public _ModelBuilder ModelBuilder => _modelBuilder ?? (_modelBuilder = new _ModelBuilder());

        protected HttpContext CurrentContext => HttpContext.Current;
        protected IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;

        public _BaseController(ILoggingService loggingService, IPagingService paggingService,
            IIdentityService identityService)
        {
            _loggingService = loggingService;
            _paggingService = paggingService;
            _identityService = identityService;
        }

        protected void AddModelError(string message) => ModelState.AddModelError(string.Empty, message);

        protected async Task<User> CurrentUser() => await _identityService.Find(User.Identity.GetUserId());

        protected string RandomString(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
    }
}