﻿using System.Web.Mvc;

namespace Elevate.Accounts.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new MyErrorFilter());
        }
    }

    public class MyErrorFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            var vr = new ViewResult
            {
                ViewName = "Error"
            };
            vr.ViewData.Model = filterContext.Exception;
            filterContext.Result = vr;
        }
    }
}