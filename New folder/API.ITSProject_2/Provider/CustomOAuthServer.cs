namespace API.ITSProject.Provider
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.OAuth;
    using Core.ObjectModels.Identity;
    using Core.ObjectModels.Identity.Enum;
    using Core.ApplicationService.Business.IdentityService;

    public class CustomOAuthServer : OAuthAuthorizationServerProvider
    {
        private readonly IIdentityService _identityService;

        public CustomOAuthServer(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            Client client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.Validated();
                return Task.FromResult<object>(null);
            }

            client = _identityService.FindClient(context.ClientId);

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format($"Client {context.ClientId} is not registered in the system"));
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != Helper.GetHash(clientSecret))
                    {
                        context.SetError("invalid_clientId", "Client secret is invalid");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            if (allowedOrigin == null)
            {
                allowedOrigin = "*";
            }


            if (!context.Response.Headers.Keys.Contains("Access-Control-Allow-Origin"))
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            }
            else
            {
                context.Response.Headers.Set("Access-Control-Allow-Origin", $"{allowedOrigin}");
            }//cors


            _IdentityData data = await _identityService.Find(context.UserName, context.Password);

            if (data.IsError)
            {
                context.SetError("invalid_grant", data.Errors[0]);
            }
            else
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                
                var props = new AuthenticationProperties(
                    new Dictionary<string, string>
                    {
                        { "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId },
                        { "userName", context.UserName },
                        { "role", await _identityService.GetRole(context.UserName) }
                    });
                context.Validated(new AuthenticationTicket(data.Data as ClaimsIdentity, props));
            }//end if check is error
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            //edit format default context's properties here: access_token, token_type, expires_in, .issued, .expires

            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }//end for add data include in AuthenticationProperties
            return Task.FromResult<object>(null);
        }
    }
}