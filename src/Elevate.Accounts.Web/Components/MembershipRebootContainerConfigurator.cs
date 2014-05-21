using System.Web;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using BrockAllen.MembershipReboot.Owin;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Owin;
using Owin;

namespace Elevate.Accounts.Web.Components
{
    public class MembershipRebootContainerConfigurator
    {
        public static void ConfigureContainer(IWindsorContainer container, IAppBuilder app, string authType)
        {
            var config = CreateMembershipRebootConfiguration(app);
            
            container.Register
            (
                Component.For<MembershipRebootConfiguration>().Instance(config),
                Component.For<IUserAccountQuery, IUserAccountRepository>()
                         .ImplementedBy<DefaultUserAccountRepository>()
                         .LifestylePerWebRequest(),
                Component.For<UserAccountService>()
                         .OnCreate(ctx =>
                         {
                             var owin = container.Resolve<IOwinContext>();
                             var debugging = false;
#if DEBUG
                                debugging = true;
#endif
                                ctx.ConfigureTwoFactorAuthenticationCookies(owin.Environment, debugging);

                         })
                         .LifestylePerWebRequest(),
                Component.For<AuthenticationService>()
                         .UsingFactoryMethod(ctx =>
                         {
                             var owin = ctx.Resolve<IOwinContext>();
                             return new OwinAuthenticationService(authType, ctx.Resolve<UserAccountService>(), owin.Environment);
                         })
                         .LifestylePerWebRequest(),
                Component.For<IOwinContext>()
                         .UsingFactoryMethod(ctx => HttpContext.Current.GetOwinContext())
            );
        }

        public static MembershipRebootConfiguration CreateMembershipRebootConfiguration(IAppBuilder app)
        {
            var config = new MembershipRebootConfiguration
            {
                RequireAccountVerification = false
            };

            config.AddEventHandler(new DebuggerEventHandler());

            var appInfo = new OwinApplicationInformation(
                app,
                "Test",
                "Test Email Signature",
                "/UserAccount/Login",
                "/UserAccount/ChangeEmail/Confirm/",
                "/UserAccount/Register/Cancel/",
                "/UserAccount/PasswordReset/Confirm/");

            var emailFormatter = new EmailMessageFormatter(appInfo);
            // uncomment if you want email notifications -- also update smtp settings in web.config
            config.AddEventHandler(new EmailAccountEventsHandler(emailFormatter));
            // uncomment to enable SMS notifications -- also update TwilloSmsEventHandler class below
            //config.AddEventHandler(new TwilloSmsEventHandler(appinfo));

            // uncomment to ensure proper password complexity
            //config.ConfigurePasswordComplexity();

            return config;
        }
    }
}