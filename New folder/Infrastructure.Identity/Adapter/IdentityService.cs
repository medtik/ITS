namespace Infrastructure.Identity.Adapter
{
    using System;
    using System.Transactions;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Security.Claims;
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Core.ObjectModels.Entities;
    using Core.ObjectModels.Identity;
    using Core.ApplicationService.Business.EntityService;
    using Core.ApplicationService.Business.IdentityService;
    using Core.ApplicationService.Database.Identities;
    using Core.ApplicationService.Business.LogService;
    using Infrastructure.Identity.Service;
    using Infrastructure.Identity.Models;
    using System.Linq;
    using Microsoft.Owin.Security.DataProtection;
    using Microsoft.AspNet.Identity.Owin;

    public class IdentityService : IIdentityService
    {
        private readonly DbContext _dbContext;
        private readonly RoleService _roleService;
        private readonly AccountService _accountService;
        private readonly ClientService _clientService;
        private readonly RefreshTokenService _refreshTokenService;

        private readonly IUserService _userService;

        private readonly ILoggingService _loggingService;

        public IdentityService(IIdentityContext identityContext, ILoggingService loggingService,
            IUserService userService, ClientService clientService, RefreshTokenService refreshTokenService)
        {
            _dbContext = identityContext.GetContext as DbContext;

            //account
            _clientService = new ClientService(identityContext);
            _refreshTokenService = new RefreshTokenService(identityContext);
            _roleService = new RoleService(new RoleStore<Role>(_dbContext));
            _accountService = new AccountService(new UserStore<Account>(_dbContext));

            //entity
            _userService = userService;

            //logging
            _loggingService = loggingService;
        }

        public async Task<_IdentityData> Create(string usernameOrEmail, string password, string fullName,
            string address, string phoneNumber, DateTimeOffset birthdate, params string[] roles)
        {
            _IdentityData identityData = null;

            try
            {
                using (TransactionScope scope = new TransactionScope
                    (TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
                {
                    identityData = new _IdentityData();

                    //create user extenstion data
                    User user = new Creator
                    {
                        FullName = fullName,
                        Birthdate = birthdate,
                        Address = address
                    };
                    int userId = _userService.CreateUser(user);
                    //create account
                    Account account = new Account
                    {
                        UserName = usernameOrEmail,
                        UserId = userId,
                        Email = usernameOrEmail,
                        PhoneNumber = phoneNumber
                    };
                    IdentityResult result = await _accountService.CreateAsync(account, password);

                    if (!result.Succeeded)
                    {
                        identityData.Errors.AddRange(result.Errors);
                    }
                    else
                    {
                        foreach (string roleName in roles)
                        {
                            if (!await _roleService.RoleExistsAsync(roleName))
                            {
                                result = await _roleService.CreateAsync(new Role { Name = roleName });

                                if (!result.Succeeded)
                                {
                                    identityData.Errors.AddRange(result.Errors);
                                }
                            }

                            if (!await _accountService.IsInRoleAsync(account.Id, roleName))
                            {
                                result = await _accountService.AddToRolesAsync(account.Id, roleName);

                                if (!result.Succeeded)
                                {
                                    identityData.Errors.AddRange(result.Errors);
                                }
                            }//check is user existed in role
                        }//crawl roles collection
                    }//is create account successful
                    if (!identityData.IsError)
                    {
                        scope.Complete();
                    }//check is error
                }//end scope
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Create), ex);
            }

            return identityData;
        }

        public async Task<_IdentityData> Find(string username, string password)
        {
            _IdentityData identityData = null;
            try
            {
                identityData = new _IdentityData();

                Account account = await _accountService.FindAsync(username ?? string.Empty, password ?? string.Empty);

                if (account == null)
                {
                    identityData.Errors.Add("Invalid username or password");
                }
                else
                {
                    ClaimsIdentity claims = await _accountService
                        .CreateIdentityAsync(account, DefaultAuthenticationTypes.ApplicationCookie);

                    identityData.Data = claims;
                }// end if check is login successful
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Find), ex);
            }

            return identityData;
        }

        public async Task<User> Find(string accountId)
        {
            Account identity = await _accountService.FindByIdAsync(accountId);
            return _userService.Find(identity.UserId);
        }

        public async Task<_IdentityData> FindAccount(string accountId)
            => new _IdentityData
            {
                Data = await _accountService.FindByIdAsync(accountId)
            };

        public async Task<_IdentityData> LoginExternalAsync(string providerName, string providerKey)
        {
            _IdentityData data = null;
            try
            {
                UserLoginInfo loginInfo = new UserLoginInfo(providerName, providerKey);

                Account account = await _accountService.FindAsyncExternal(loginInfo);
                if (account != null)
                {
                    data = new _IdentityData()
                    {
                        Data = account
                    };
                }
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(LoginExternalAsync), ex);
            }

            return data;
        }

        #region For refresh token
        public Client FindClient(string clientId)
            => _clientService.FindClient(clientId);

        public async Task<bool> AddRefreshToken(RefreshToken token)
            => await _refreshTokenService.AddRefreshToken(token);

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
            => await _refreshTokenService.RemoveRefreshToken(refreshTokenId);

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
            => await _refreshTokenService.RemoveRefreshToken(refreshToken);

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
            => await _refreshTokenService.FindRefreshToken(refreshTokenId);

        public IEnumerable<RefreshToken> GetAllRefreshTokens()
            => _refreshTokenService.GetAllRefreshTokens();

        public _IdentityData FindAccount(int userId)
        {
            return new _IdentityData()
            {
                Data = _dbContext.Set<Account>().FirstOrDefault(_ => _.UserId == userId)
            };
        }

        public async Task<string> GetFullName(string username)
        {
            Account identity = await _accountService.FindByNameAsync(username);
            if (identity != null)
            {
                return _userService.Find(identity.UserId) != null ? _userService.Find(identity.UserId).FullName : null;
            }
            else
                return null;
        }

        public async Task<bool> ChangePassword(string username, string newPassword)
        {
            try
            {
                Account identity = await _accountService.FindByNameAsync(username);
                string resetToken = await _accountService.GeneratePasswordResetTokenAsync(identity.Id);

                var user = await this._accountService.ResetPasswordAsync(identity.Id, resetToken, newPassword);

                return user.Succeeded;
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(ChangePassword), ex);

                return false;
            }
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public async Task<string> GetRole(string username)
        {
            Account identity = _dbContext.Set<Account>().Include(_ => _.Roles).FirstOrDefault(_ => _.UserName == username);

            if (identity != null)
            {
                string roleId = identity.Roles.FirstOrDefault().RoleId;
                return (await _roleService.FindByIdAsync(roleId)).Name;
            }
            else
                return null;
        }

        public async Task<_IdentityData> FindAsync(string provider, string userId)
        {
            _IdentityData data = new _IdentityData();
            data.Data = await _accountService.FindAsync(new UserLoginInfo(provider, userId)) as Account;
            return data;
        }

        public async Task<_IdentityData> CreateAsync(string UserName)
        {
            User user = new Creator
            {
                FullName = UserName
            };
            int userId = _userService.CreateUser(user);
            return new _IdentityData()
            {
                Data = await _accountService.CreateAsync(new Account
                {
                    UserName = UserName,
                    UserId = userId,
                    Email = "defaultEmail@gmail.com"
                })
            };
        }

        public async Task<_IdentityData> AddLoginAsync(string userId, string a, string b)
        {
            try
            {
                UserLoginInfo login = new UserLoginInfo(a, b);
                _IdentityData result = new _IdentityData();
                IdentityResult reuslttest = await _accountService.AddLoginAsync(userId, login);
                result.Data = reuslttest;
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}