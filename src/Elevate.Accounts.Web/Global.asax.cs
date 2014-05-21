using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Elevate.Accounts.Web.App_Start;
using Elevate.Accounts.Web.Components;
using log4net;

namespace Elevate.Accounts.Web
{
    public class ElevateAccountsWebApplication : HttpApplication
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(ElevateAccountsWebApplication));

        public const string Name = "Elevate.Accounts.Web";

        protected void Application_Start()
        {
            MvcConfigurator.ConfigureContainer(Startup.Container);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        void Application_BeginRequest()
        {
            //Thread.CurrentThread.CurrentCulture =
            //    Thread.CurrentThread.CurrentUICulture =
            //    CultureInfo.CreateSpecificCulture("ru-ru");
        }
    }
}