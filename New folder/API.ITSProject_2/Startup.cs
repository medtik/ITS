using Microsoft.Owin;

[assembly: OwinStartup(typeof(API.ITSProject_2.Startup))]
namespace API.ITSProject_2
{
    using System;
    using System.Reflection;
    using System.Web.Http;
    using System.Collections.Generic;
    using Owin;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Cors;
    using Microsoft.Owin.Security.OAuth;
    using Microsoft.Owin.Security.Facebook;
    using Ninject;
    using Ninject.Modules;
    using Ninject.Web.Common.OwinHost;
    using Ninject.Web.WebApi.OwinHost;
    using Core.ApplicationService.Business.IdentityService;
    using DependencyResolver;
    using API.ITSProject_2.Provider;
    using API.ITSProject_2.Provider.ExternalLogin;
    using Microsoft.Owin.Security.DataProtection;

    public class Startup
    {
        internal static IDataProtectionProvider DataProtectionProvider;

        private static IKernel _kernel;
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }

        private static IKernel CreateKernel() => Kernel;

        public static IKernel Kernel
        {
            get
            {
                if (_kernel == null)
                {
                    _kernel = new StandardKernel();
                    _kernel.Load(Assembly.GetExecutingAssembly());

                    _kernel.Load(new List<NinjectModule>()
                    {
                        new InfrastructureModules(),
                        new ServiceModules()
                    });
                }//end if check is kernel created

                return _kernel;
            }
        }

        public void Configuration(IAppBuilder app)
        {
            DataProtectionProvider = app.GetDataProtectionProvider();
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            SwaggerConfig.Register(config);

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),//define the token path
                AllowInsecureHttp = true,//for debug environment,
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),//declare the expire time
                Provider = new CustomOAuthServer(Kernel.Get<IIdentityService>()),
                RefreshTokenProvider = new CustomRefreshTokenServer(Kernel.Get<IIdentityService>())
            });

            app.UseCors(CorsOptions.AllowAll);
            //token generation
            #region facebook
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseFacebookAuthentication(facebookAuthOptions = new FacebookAuthenticationOptions
            {
                AppId = "266318357470729",
                AppSecret = "c425f789382947f98760dc4b55ca6a9f",
                Provider = new FacebookAuthServer()
            });
            #endregion

            app.UseOAuthBearerAuthentication(OAuthBearerOptions = new OAuthBearerAuthenticationOptions());
            app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(config);
        }
    }
}