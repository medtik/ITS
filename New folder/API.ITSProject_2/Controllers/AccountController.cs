namespace API.ITSProject_2.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Core.ObjectModels.Identity;
    using Core.ApplicationService.Business.IdentityService;
    using Core.ApplicationService.Business.LogService;
    using Core.ApplicationService.Business.PagingService;
    using API.ITSProject_2.Resource;
    using API.ITSProject_2.ViewModels;
    using System.Net.Http;
    using System.Security.Claims;
    using Infrastructure.Identity.Models;
    using System.Linq;
    using Core.ApplicationService.Business.MailService;
    using Core.ApplicationService.Business.EntityService;
    using System.Web;
    using System.Collections.Generic;

    public class AccountController : _BaseController
    {
        private readonly IMailService _mailService;

        public AccountController(IMailService mailService, ILoggingService loggingService, IPagingService paggingService,
            IIdentityService _identityService, IPhotoService photoService) : base(loggingService, paggingService, _identityService, photoService)
        {
            _mailService = mailService;
        }

        #region Helper
        //private string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        //{

        //    Uri redirectUri;

        //    var redirectUriString = GetQueryString(Request, "redirect_uri");

        //    if (string.IsNullOrWhiteSpace(redirectUriString))
        //    {
        //        return "redirect_uri is required";
        //    }

        //    bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

        //    if (!validUri)
        //    {
        //        return "redirect_uri is invalid";
        //    }

        //    var clientId = GetQueryString(Request, "client_id");

        //    if (string.IsNullOrWhiteSpace(clientId))
        //    {
        //        return "client_Id is required";
        //    }

        //    //var client =  (.FindAccount(clientId).Data) as Account;

        //    //if (client == null)
        //    //{
        //    //    return string.Format("Client_id '{0}' is not registered in the system.", clientId);
        //    //}

        //    //if (!string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
        //    //{
        //    //    return string.Format("The given URL is not allowed by Client_id '{0}' configuration.", clientId);
        //    //}

        //    redirectUriOutput = redirectUri.AbsoluteUri;

        //    return string.Empty;

        //}

        //private string GetQueryString(HttpRequestMessage request, string key)
        //{
        //    var queryStrings = request.GetQueryNameValuePairs();

        //    if (queryStrings == null) return null;

        //    var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

        //    if (string.IsNullOrEmpty(match.Value)) return null;

        //    return match.Value;
        //}
        #endregion

        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null) return null;

            var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

            if (string.IsNullOrEmpty(match.Value)) return null;

            return match.Value;
        }

        [HttpPut]
        [Route("api/account/BanAccount")]
        public IHttpActionResult BanUser(int userId)
        {
            try
            {
                var account = _identityService.FindAccount(userId).Data as Account;
                _identityService.LoginAvaiable(account.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(BanUser), ex);
                return InternalServerError();
            }
        }

        [AllowAnonymous]
        [HttpPost, Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Account user = null;
                if (string.IsNullOrWhiteSpace(model.email) || model.email.ToLower() == "none")
                {
                    user = (await _identityService.FindByUsername(model.uid)).Data as Account;
                } else
                {
                    user = (await _identityService.FindByUsername(model.email)).Data as Account;
                }

                bool hasRegistered = user != null;

                if (!hasRegistered)
                {
                    if (model.email != "None")
                    {
                        await _identityService.Create(model.email, "abcdefghijklmnopqrstuvwxyz", model.displayName, model.photoUrl, "Photo", DateTimeOffset.Now, nameof(RoleType.User));
                    } else
                    {
                        await _identityService.Create(model.uid, "abcdefghijklmnopqrstuvwxyz", model.displayName, model.photoUrl, string.Empty, DateTimeOffset.Now, nameof(RoleType.User));
                    }
                }
                var client = new HttpClient();

                string baseAddress = "http://" + HttpContext.Current.Request.Url.Authority;
                client.BaseAddress = new Uri(baseAddress);
                HttpContent content = null;
                if (model.email == "None")
                {
                    content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
                        new KeyValuePair<string, string>("username", model.uid),
                        new KeyValuePair<string, string>("password", "abcdefghijklmnopqrstuvwxyz"),
                        new KeyValuePair<string, string>("grant_type", "password")
                    });
                }
                else
                {
                    content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
                        new KeyValuePair<string, string>("username", model.email),
                        new KeyValuePair<string, string>("password", "abcdefghijklmnopqrstuvwxyz"),
                        new KeyValuePair<string, string>("grant_type", "password")
                    });
                }
                    
                HttpResponseMessage response = await client.PostAsync("token", content);
                string message = await response.Content.ReadAsStringAsync();
                //System.Uri.UnescapeDataString(message);
                return Ok(message);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(RegisterExternal), ex);
                throw ex;
            }

        }

        [HttpPost]
        [Route("api/Account/RegisterAdmin")]
        public async Task<IHttpActionResult> RegisterAdmin(RegisterViewModels register)
        {
            try
            {
                var year = UserController.DateDiffYears(register.Birthdate, DateTimeOffset.Now);
                if (year <= 10)
                    ModelState.AddModelError(string.Empty, "Ngày sinh phải cách hiện tại 10 năm");
                if (ModelState.IsValid)
                {
                    _IdentityData result = await _identityService.Create(register.EmailAddress, register.Password,
                                            register.FullName, register.Address, register.PhoneNumber,
                                            register.Birthdate, nameof(RoleType.Administrator));

                    if (result.IsError)
                    {
                        base.AddModelError(result.Errors[0]);

                        return BadRequest(ModelState);
                    }
                    else
                    {
                        return Ok();
                    }//end if check is existed errors
                }
                else
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

        public async Task<IHttpActionResult> Register(RegisterViewModels register)
        {
            try
            {
                var year = UserController.DateDiffYears(register.Birthdate, DateTimeOffset.Now);
                if (year <= 10)
                    ModelState.AddModelError(string.Empty, "Ngày sinh phải cách hiện tại 10 năm");
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
                }
                else
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

        [HttpPut]
        [Route("api/Account/ChangeRole")]
        public async Task<IHttpActionResult> ChangeRole(int userId)
        {
            Account data = (_identityService.FindAccount(userId)).Data as Account;
            if (data != null)
            {
                await _identityService.ChangeRole(data.Id);
            }
            else
            {
                return BadRequest();
            }
            return Ok();
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