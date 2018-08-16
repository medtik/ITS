namespace Infrastructure.Identity.Service
{
    using System.Linq;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Core.ObjectModels.Identity;
    using Core.ApplicationService.Database.Identities;

    public class RefreshTokenService
    {
        private DbContext _dbContext;

        public RefreshTokenService(IIdentityContext context)
        {
            _dbContext = context.GetContext as DbContext;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken = _dbContext.Set<RefreshToken>().Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _dbContext.Set<RefreshToken>().Add(token);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _dbContext.Set<RefreshToken>().FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _dbContext.Set<RefreshToken>().Remove(refreshToken);
                return await _dbContext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _dbContext.Set<RefreshToken>().Remove(refreshToken);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _dbContext.Set<RefreshToken>().FindAsync(refreshTokenId);

            return refreshToken;
        }

        public IEnumerable<RefreshToken> GetAllRefreshTokens()
        {
            return _dbContext.Set<RefreshToken>().ToList();
        }
    }
}