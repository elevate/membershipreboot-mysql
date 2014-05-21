using System.Security.Claims;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Elevate.Accounts.Web.Core;
using Owin;

namespace Elevate.Accounts.Web.Components
{
    public static class MvcConfigurator
    {
        public static void UseMvc(this IAppBuilder builder, IWindsorContainer container)
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            AreaRegistration.RegisterAllAreas();

            ConfigureContainer(Startup.Container);
            
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        public static void ConfigureContainer(IWindsorContainer container)
        {
            var assembly = typeof(Startup).Assembly;
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            container.RegisterControllers(assembly);
            container.Register
            (
                Component.For<IControllerFactory>().Instance(controllerFactory)
            );
        }
    }

}