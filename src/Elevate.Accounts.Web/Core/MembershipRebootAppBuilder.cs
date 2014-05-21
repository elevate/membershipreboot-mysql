using System.Data.Entity;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using BrockAllen.MembershipReboot.Owin;
using Castle.Windsor;
using Elevate.Accounts.Web.Components;
using Elevate.Accounts.Web.Data.Migrations;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Elevate.Accounts.Web.Core
{
    public static class MembershipRebootAppBuilder
    {
        public static void UseElevateMembership(this IAppBuilder builder, IWindsorContainer container)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DefaultMembershipRebootDatabase, Configuration>());

            //System.Data.Entity.Database.SetInitializer(new System.Data.Entity.CreateDatabaseIfNotExists<DefaultMembershipRebootDatabase>());

            var cookieOptions = new CookieAuthenticationOptions
            {
                AuthenticationType = MembershipRebootOwinConstants.AuthenticationType
            };

            MembershipRebootContainerConfigurator.ConfigureContainer(container, builder, cookieOptions.AuthenticationType);

            builder.UseMembershipReboot(cookieOptions, 
                env =>
                {
                    return container.Resolve<UserAccountService<UserAccount>>();
                }, 
                env =>
                {
                    return container.Resolve<AuthenticationService<UserAccount>>();
                }
            );
        }
    }
}