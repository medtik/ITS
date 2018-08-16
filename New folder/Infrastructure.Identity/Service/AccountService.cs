namespace Infrastructure.Identity.Service
{
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Infrastructure.Identity.Models;
    using Microsoft.AspNet.Identity.Owin;
    using System;

    public class AccountService : UserManager<Account>
    {
        public AccountService(IUserStore<Account> store) : base(store)
        {

            var machineKeyDataProtector = new MachineKeyDataProtector("ResetPasswordPurpose");
            this.UserTokenProvider = new DataProtectorTokenProvider<Account>(machineKeyDataProtector)
            {
                TokenLifespan = TimeSpan.FromHours(24),
            };

            //config validator for user model
            this.UserValidator = new UserValidator<Account>(this)
            {
                RequireUniqueEmail = true,
                AllowOnlyAlphanumericUserNames = false
            };
        }

        public async Task<Account> FindByNameOrEmailAsync(string usernameOrEmail, string password)
        {
            string username = usernameOrEmail;

            if (usernameOrEmail.Contains("@"))
            {
                Account user = await base.FindByEmailAsync(usernameOrEmail);

                if (user != null)
                {
                    username = user.UserName;
                }//find username by email
            }

            return await FindAsync(username, password);
        }

        public async Task<Account> FindAsyncExternal(UserLoginInfo loginInfo)
            => await FindAsync(loginInfo);

        public async Task<IdentityResult> CreateAsyncExternal(Account user) => await CreateAsync(user);

        public async Task<IdentityResult> AddLoginAsyncExternal(string userId, UserLoginInfo login) 
            => await AddLoginAsync(userId, login);
    }
}