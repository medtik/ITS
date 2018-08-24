namespace API.ITSProject_2.Provider.ExternalLogin
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class _ExternalOAuthServer : IHttpActionResult
    {
        public string ProviderName { get; set; }

        public HttpRequestMessage Request { get; set; }

        public _ExternalOAuthServer(string providerName, HttpRequestMessage request)
        {
            this.ProviderName = providerName;
            this.Request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            this.Request.GetOwinContext().Authentication.Challenge(ProviderName);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            return Task.FromResult(response);
        }
    }
}