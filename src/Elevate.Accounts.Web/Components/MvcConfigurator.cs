using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Elevate.Accounts.Web.Core;

namespace Elevate.Accounts.Web.Components
{
    public class MvcConfigurator
    {
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