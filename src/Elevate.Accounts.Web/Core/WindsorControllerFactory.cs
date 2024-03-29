﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;

namespace Elevate.Accounts.Web.Core
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        readonly IKernel _kernel;

        public WindsorControllerFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        public override void ReleaseController(IController controller)
        {
            _kernel.ReleaseComponent(controller);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, string.Format("The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path));
            }

            var controller = _kernel.Resolve(controllerType) as Controller;
            if (controller == null)
            {
                return null;
            }

            return controller;
        }
    }
}