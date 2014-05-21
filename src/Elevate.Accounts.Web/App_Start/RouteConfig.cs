using System.Web.Mvc;
using System.Web.Routing;

namespace Elevate.Accounts.Web
{
    public class IgnoreRoutes
    {
        public static void Set(RouteCollection routes)
        {
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Content");
        }
    }

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            IgnoreRoutes.Set(routes); 
            
            routes.MapMvcAttributeRoutes();
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Elevate.Accounts.Web.Controllers" }
            );
        }
    }
}