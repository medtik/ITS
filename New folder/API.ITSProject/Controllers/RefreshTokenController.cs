namespace API.ITSProject.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using Core.ApplicationService.Business.IdentityService;
    using Core.ApplicationService.Business.LogService;
    using Core.ApplicationService.Business.PagingService;

    public class RefreshTokenController : _BaseController
    {
        public RefreshTokenController(ILoggingService loggingService, IPagingService paggingService, IIdentityService identityService) : base(loggingService, paggingService, identityService)
        {
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult Get()
        {
            return Ok(_identityService.GetAllRefreshTokens());
        }

        [HttpDelete]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Delete(string tokenId)
        {
            var result = await _identityService.RemoveRefreshToken(tokenId);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Token Id does not exist");

        }
    }
}