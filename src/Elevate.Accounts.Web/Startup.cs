using BrockAllen.MembershipReboot;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Elevate.Accounts.Web;
using Elevate.Accounts.Web.Components;
using Elevate.Accounts.Web.Core;
using Elevate.Accounts.Web.Services;
using log4net;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Elevate.Accounts.Web
{
    public class Startup
    {
        protected readonly ILog Logger = LogManager.GetLogger(typeof(Startup));

        internal static readonly IWindsorContainer Container = new WindsorContainer();

        public void Configuration(IAppBuilder app)
        {
            app.Use(async (ctx, next) =>
            {
                using (var scope = Container.BeginScope())
                {
                    ctx.Environment.SetUserAccountService(() => Container.Resolve<UserAccountService>());
                    await next();
                }
            });

            app.UseElevateAuthorizationServer(Container);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "External", 
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Passive
            });
            app.UseGoogleAuthentication(new GoogleAuthenticationOptions
            {
                SignInAsAuthenticationType ="External"
            });
            app.UseElevateMembership(Container);
            app.UseMvc(Container);
        }
    }
}