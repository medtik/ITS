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
    using System;
    using System.IO;
    using System.Drawing;
    using Core.ApplicationService.Business.EntityService;
    using System.Drawing.Imaging;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text.RegularExpressions;
    using System.Linq;

    public abstract class _BaseController : ApiController
    {
        private _ModelBuilder _modelBuilder;
        protected readonly IPhotoService _photoService;
        protected readonly ILoggingService _loggingService;
        protected readonly IPagingService _paggingService;

        protected readonly IIdentityService _identityService;

        public _ModelBuilder ModelBuilder => _modelBuilder ?? (_modelBuilder = new _ModelBuilder());

        protected HttpContext CurrentContext => HttpContext.Current;
        protected IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;

        public _BaseController(ILoggingService loggingService, IPagingService paggingService,
            IIdentityService identityService, IPhotoService photoService)
        {
            _loggingService = loggingService;
            _paggingService = paggingService;
            _identityService = identityService;
            _photoService = photoService;
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

        protected byte[] ConvertToStream(int photoId)
        {
            string base64encodedstring = _photoService.GetBase64(photoId);
            //string converted = base64encodedstring.Replace('-', '+');
            //converted = converted.Replace('_', '/');
            var imageParts = base64encodedstring.Split(',').ToList<string>();
            //Exclude the header from base64 by taking second element in List.
            byte[] bytes = Convert.FromBase64String(imageParts[1]);
            //var bytes = Convert.FromBase64String(response);
            return bytes;
        }

       
    }
}