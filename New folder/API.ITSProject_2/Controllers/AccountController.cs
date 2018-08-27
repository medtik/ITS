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
    using API.ITSProject_2.Provider.ExternalLogin;
    using System.Net.Http;
    using System.Security.Claims;
    using Infrastructure.Identity.Models;
    using System.Linq;
    using Core.ApplicationService.Business.MailService;
    using Microsoft.Owin.Security.DataProtection;
    using System.IO;
    using Core.ApplicationService.Business.EntityService;
    using Newtonsoft.Json.Linq;
    using Microsoft.Owin.Security.OAuth;
    using Microsoft.Owin.Security;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Web;
    using System.Collections.Generic;
    using Newtonsoft.Json;

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
                }
                else
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

            var client = _identityService.FindClient(clientId);

            if (client == null)
            {
                return string.Format("Client_id '{0}' is not registered in the system.", clientId);
            }

            if (!string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("The given URL is not allowed by Client_id '{0}' configuration.", clientId);
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

        private async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(string provider, string accessToken)
        {
            ParsedExternalAccessToken parsedToken = null;

            var verifyTokenEndPoint = "";

            if (provider == "Facebook")
            {
                var appToken = "266318357470729";
                verifyTokenEndPoint = string.Format("https://graph.facebook.com/debug_token?input_token={1}&access_token={0}", accessToken, appToken);
            }
            else if (provider == "Google")
            {
                verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
            }
            else
            {
                return null;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                parsedToken = new ParsedExternalAccessToken();

                if (provider == "Facebook")
                {
                    parsedToken.user_id = jObj["data"]["application"];
                    parsedToken.app_id = jObj["data"]["app_id"];

                    if (!string.Equals(Startup.facebookAuthOptions.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
            }
            return parsedToken;
        }

        private JObject GenerateLocalAccessTokenResponse(string userName)
        {

            var tokenExpiration = TimeSpan.FromDays(1);

            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim("role", "user"));

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            JObject tokenResponse = new JObject(
                                        new JProperty("userName", userName),
                                        new JProperty("access_token", accessToken),
                                        new JProperty("token_type", "bearer"),
                                        new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                                        new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                                        new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
        );

            return tokenResponse;
        }

        // POST api/Account/RegisterExternal
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
                Account user = (await _identityService.FindAsync(model.Provider, model.uid)).Data as Account;

                bool hasRegistered = user != null;

                if (!hasRegistered)
                {
                    await _identityService.Create(model.uid, "abcdefghijklmnopqrstuvwxyz", model.displayName, model.photoUrl, "Photo", DateTimeOffset.Now);
                }
                var client = new HttpClient();

                string baseAddress = "http://" + HttpContext.Current.Request.Url.Authority;
                client.BaseAddress = new Uri(baseAddress);
                HttpContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
                        new KeyValuePair<string, string>("username", model.uid),
                        new KeyValuePair<string, string>("password", "abcdefghijklmnopqrstuvwxyz"),
                        new KeyValuePair<string, string>("grant_type", "password")
                    });
                HttpResponseMessage response = await client.PostAsync("token", content);
                string message = await response.Content.ReadAsStringAsync();
                
                return Ok(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ObtainLocalAccessToken")]
        public async Task<IHttpActionResult> ObtainLocalAccessToken(string provider, string externalAccessToken)
        {

            if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
            {
                return BadRequest("Provider or external access token is not sent");
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(provider, externalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            Account user = (await _identityService.FindAsync(provider, verifiedAccessToken.user_id)).Data as Account;

            bool hasRegistered = user != null;

            if (!hasRegistered)
            {
                return BadRequest("External user is not registered");
            }

            //generate access token response
            var accessTokenResponse = GenerateLocalAccessTokenResponse(user.UserName);

            return Ok(accessTokenResponse);

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