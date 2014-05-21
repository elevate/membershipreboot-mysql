using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using Castle.Windsor;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Elevate.Accounts.Web.Services
{
    public class ElevateAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        readonly IWindsorContainer _container;

        public ElevateAuthorizationServerProvider(IWindsorContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Validate That The Client, i.e. Phone, Browser, Api Has The Authority To Request Tokens
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);

            string cid, csecret;
            if (context.TryGetBasicCredentials(out cid, out csecret))
            {
                var svc = context.OwinContext.Environment.GetUserAccountService<UserAccount>();
                if (svc.Authenticate("clients", cid, csecret))
                {
                    context.Validated();
                }
            }
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Validate That The Client, i.e. Phone, Browser Etc Is Allowed To Access The Scopes Requested
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateTokenRequest(OAuthValidateTokenRequestContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);

            if (context.TokenRequest.IsResourceOwnerPasswordCredentialsGrantType)
            {
                var svc = context.OwinContext.Environment.GetUserAccountService<UserAccount>();
                var client = svc.GetByUsername("clients", context.ClientContext.ClientId);
                var scopes = context.TokenRequest.ResourceOwnerPasswordCredentialsGrant.Scope;
                if (scopes.All(scope => client.HasClaim("scope", scope)))
                {
                    context.Validated();
                }
            }
            return Task.FromResult<object>(null);
        }

        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var authentication = context.OwinContext.Authentication;
            if (authentication == null)
            {
                return base.GrantClientCredentials(context);
            }
            var user = authentication.User;
            if (user == null)
            {
                return base.GrantClientCredentials(context);
            }
            
            var userAccountService = context.OwinContext.Environment.GetUserAccountService<UserAccount>();
            var account = userAccountService.GetByUsername("users", user.Identity.Name);
            if (account == null)
            {
                return base.GrantClientCredentials(context);
            }
            
            var claims = account.GetAllClaims();
            var id = new System.Security.Claims.ClaimsIdentity(claims, "MembershipReboot");
            context.Validated(id);

            return base.GrantClientCredentials(context);
        }

        /// <summary>
        /// Authenticate The User, Create A ClaimsIdentity Return The Token Containing The Claims
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userAccountService = context.OwinContext.Environment.GetUserAccountService<UserAccount>();

            UserAccount user;
            if (userAccountService.AuthenticateWithEmail("users", context.UserName, context.Password, out user))
            {
                var claims = user.GetAllClaims();

                var id = new System.Security.Claims.ClaimsIdentity(claims, "MembershipReboot");
                context.Validated(id);
            }

            return base.GrantResourceOwnerCredentials(context);
        }
    }
}