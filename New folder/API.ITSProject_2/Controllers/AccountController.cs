namespace API.ITSProject.Controllers
{
    using System;
    using System.IO;
    using System.Web.Http;
    using System.Threading.Tasks;
    using Core.ObjectModels.Identity;
    using Core.ApplicationService.Business.MailService;
    using Core.ApplicationService.Business.EntityService;
    using Core.ApplicationService.Business.IdentityService;
    using Core.ApplicationService.Business.LogService;
    using Core.ApplicationService.Business.PagingService;
    using API.ITSProject.Resource;
    using API.ITSProject.ViewModels;
    using Microsoft.AspNet.Identity;
    using API.ITSProject.Provider.ExternalLogin;
    using System.Security.Claims;
    using System.Net.Http;
    using System.Linq;
    using Infrastructure.Identity.Models;

    public class AccountController : _BaseController
    {
        private readonly IMailService _mailService;

        public AccountController(IMailService mailService, ILoggingService loggingService, IPagingService paggingService,
            IIdentityService _identityService, IPhotoService photoService) : base(loggingService, paggingService, _identityService, photoService)
        {
            _mailService = mailService;
        }

        #region Helper
        private string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        {

            Uri redirectUri;

            var redirectUriString = GetQueryString(Request, "redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return "redirect_uri is required";
            }

            bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

            if (!validUri)
            {
                return "redirect_uri is invalid";
            }

            var clientId = GetQueryString(Request, "client_id");

            if (string.IsNullOrWhiteSpace(clientId))
            {
                return "client_Id is required";
            }

            var client = _identityService.FindClient(clientId) as Client;

            if (client == null)
            {
                return string.Format("client_id '{0}' is not registered in the system.", clientId);
            }

            if (!string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("the given url is not allowed by client_id '{0}' configuration.", clientId);
            }

            redirectUriOutput = redirectUri.AbsoluteUri;

            return string.Empty;

        }

        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null) return null;

            var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

            if (string.IsNullOrEmpty(match.Value)) return null;

            return match.Value;
        }
        #endregion

        [HttpPost]
        [Route("api/Account/RecoverPassword")]
        public async Task<IHttpActionResult> RecoverPassword(UserRecoverPassword userRecover)
        {
            try
            {
                var fullName = await _identityService.GetFullName(userRecover.Email);
                var newPassword = RandomString(8);
                if (!string.IsNullOrEmpty(fullName))
                {
                    bool result = await _identityService.ChangePassword(userRecover.Email, newPassword);

                    if (result)
                    {
                        string content = File.ReadAllText(base.CurrentContext.Server.MapPath("/Resource/RecoverMail.html"));
                        content = content.Replace("{{UserName}}", fullName);
                        content = content.Replace("{{NewPassword}}", $"{newPassword}");
                        result = _mailService.SendMail(userRecover.Email, "Khôi phục mật khẩu", content);

                        if (result)
                            return Ok();
                        else
                            return InternalServerError(new Exception("Can't send email"));
                    }
                } else
                {
                    return BadRequest("User not existed in system");
                }
                return InternalServerError(new Exception("Unknown error"));
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(RecoverPassword), ex);
                return InternalServerError(ex);
            }
        }

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            string redirectUri = string.Empty;

            if (error != null)
            {
                return BadRequest(Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new _ExternalOAuthServer(provider, this.Request);
            }

            var redirectUriValidationResult = ValidateClientAndRedirectUri(this.Request, ref redirectUri);

            if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
            {
                return BadRequest(redirectUriValidationResult);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new _ExternalOAuthServer(provider, this.Request);
            }

            var user = await _identityService.LoginExternalAsync(externalLogin.LoginProvider, externalLogin.ProviderKey);

            bool hasRegistered = user != null;

            redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}",
                                            redirectUri,
                                            externalLogin.ExternalAccessToken,
                                            externalLogin.LoginProvider,
                                            hasRegistered.ToString(),
                                            externalLogin.UserName);

            return Redirect(redirectUri);
        }

        public async Task<IHttpActionResult> Register(RegisterViewModels register)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _IdentityData result = await _identityService.Create(register.EmailAddress, register.Password,
                                            register.FullName, register.Address, register.PhoneNumber,
                                            register.Birthdate, nameof(RoleType.User));

                    if (result.IsError)
                    {
                        base.AddModelError(result.Errors[0]);

                        return BadRequest(ModelState);
                    }
                    else
                    {
                        return Ok();
                    }//end if check is existed errors
                } else
                {
                    return BadRequest(ModelState);
                }
                
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Register), ex);

                return InternalServerError(ex);
            }
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }
            public string ExternalAccessToken { get; set; }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer) || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name),
                    ExternalAccessToken = identity.FindFirstValue("ExternalAccessToken"),
                };
            }
        }
    }
}