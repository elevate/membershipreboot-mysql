using System;
using System.Reflection;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Elevate.Accounts.Web.Core
{
    public static class WindsorExtensions
    {
        public static IWindsorContainer RegisterController<T>(this IWindsorContainer container) where T : IController
        {
            container.RegisterControllers(typeof(T));
            return container;
        }

        public static IWindsorContainer RegisterControllers(this IWindsorContainer container, params Type[] controllerTypes)
        {
            foreach (var type in controllerTypes)
            {
                if (type.IsController())
                {
                    container.RegisterController(type);
                }
            }

            return container;
        }

        public static void RegisterController(this IWindsorContainer container, Type type)
        {
            container.Register
            (
                Component.For(type).ImplementedBy(type).Named(type.FullName.ToLower()).LifestylePerWebRequest()
            );
        }

        public static IWindsorContainer RegisterControllers(this IWindsorContainer container, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
                container.RegisterControllers(assembly.GetExportedTypes());

            return container;
        }
    }
}