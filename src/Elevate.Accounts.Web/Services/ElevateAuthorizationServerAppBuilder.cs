using Castle.Windsor;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Elevate.Accounts.Web.Services
{
    public static class ElevateAuthorizationServerAppBuilder
    {
        public static void UseElevateAuthorizationServer(this IAppBuilder builder, IWindsorContainer container)
        {
            var oauthServerConfig = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                Provider = new ElevateAuthorizationServerProvider(container),
                TokenEndpointPath = new PathString("/token"),
            };
            builder.UseOAuthAuthorizationServer(oauthServerConfig);
        }
    }
}