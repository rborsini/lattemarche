using LatteMarche.Application.Logs.Interfaces;
using LatteMarche.WebApi.App_Start;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LatteMarche.WebApi.Filters
{
    public class MvcExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public ILogsService logsService { get; set; }

        private static ILog log = LogManager.GetLogger(typeof(MvcExceptionFilter));

        void IExceptionFilter.OnException(ExceptionContext filterContext)
        {
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];

            var message = String.Format("{0}/{1} [UNHANDLED EXCEPTION]", controllerName, actionName);

            this.logsService.Create(new Application.Logs.Dtos.LogRecordDto()
            {
                Date = DateTime.Now,
                Thread = "service",
                Level = "ERROR",
                Logger = "mvc",
                Identity = filterContext.RequestContext.HttpContext.User.Identity.Name,
                Message = message
            });

            log.Error($"{filterContext.RequestContext.HttpContext.User.Identity.Name} - {message}");

        }
    }
}