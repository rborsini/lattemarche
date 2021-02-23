using LatteMarche.Application.Logs.Interfaces;
using LatteMarche.WebApi.App_Start;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace LatteMarche.WebApi.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private static ILog log = LogManager.GetLogger(typeof(ApiExceptionFilter));

        public ILogsService logsService { get; set; }

        public override void OnException(HttpActionExecutedContext filterContext)
        {
            var routeData = filterContext.ActionContext.ControllerContext.RouteData;
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];

            var message = String.Format("{0}/{1} [UNHANDLED EXCEPTION]", controllerName, actionName);

            var request = filterContext.Request.ToString();
            var arguments = JsonConvert.SerializeObject(filterContext.ActionContext.ActionArguments);

            this.logsService.Create(new Application.Logs.Dtos.LogRecordDto()
            {
                Date = DateTime.Now,
                Thread = "service",
                Level = "ERROR",
                Logger = "api",
                Identity = HttpContext.Current.User.Identity.Name,
                Message = message,
                Request = request,
                Arguments = arguments,
                Exception = $"{filterContext.Exception}"
            });

            log.Error(message, filterContext.Exception);
        }

    }
}