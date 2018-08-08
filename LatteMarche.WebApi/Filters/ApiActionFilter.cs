using LatteMarche.Application.Logs.Interfaces;
using LatteMarche.WebApi.App_Start;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace LatteMarche.WebApi.Filters
{
    public class ApiActionFilter : ActionFilterAttribute
    {
        private Stopwatch swAction = new Stopwatch();

        //public ILogsService logsService { get; set; }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            swAction.Restart();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            swAction.Stop();

            var routeData = actionExecutedContext.ActionContext.ControllerContext.RouteData;

            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];

            var message = String.Format("{0}/{1} [{2} sec]", controllerName, actionName, swAction.Elapsed.ToString("s\\.f"));

            var request = actionExecutedContext.Request.ToString();
            var arguments = JsonConvert.SerializeObject(actionExecutedContext.ActionContext.ActionArguments);

            LoggerConfig.ApiLog.Info(message, swAction.Elapsed.TotalMilliseconds, request, arguments);

            //this.logsService.Create(new Application.Logs.Dtos.LogRecordDto()
            //{
            //    Date = DateTime.Now,
            //    Thread = "service",
            //    Level = "INFO",
            //    Logger = "api",
            //    Identity = HttpContext.Current.User.Identity.Name,
            //    Message = message,
            //    Request = request,
            //    Arguments = arguments
            //});

            Debug.WriteLine(message, "API Action Filter Log");
        }

    }
}