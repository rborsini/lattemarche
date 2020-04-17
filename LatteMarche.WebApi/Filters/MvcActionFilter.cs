using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Application.Logs.Interfaces;
using LatteMarche.Core;
using LatteMarche.WebApi.App_Start;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LatteMarche.WebApi.Filters
{
    public class MvcActionFilter : ActionFilterAttribute
    {
        private Stopwatch swAction = new Stopwatch();
        private Stopwatch swResult = new Stopwatch();

        private string actionParameters;
        private string identity;

        // http://docs.autofac.org/en/latest/integration/mvc.html
        public IUnitOfWork Uow { get; set; }

        public IAutorizzazioniService autorizzazioniService { get; set; }

        public ILogsService logsService { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            swAction.Restart();

            ControllerBase controller = filterContext.Controller;

            if (controller != null)
            {
                this.identity = filterContext.RequestContext.HttpContext.User.Identity.Name;
                var session = filterContext.HttpContext.Session;
                var controllerName = filterContext.RouteData.Values["controller"].ToString();
                var actionName = filterContext.RouteData.Values["action"].ToString();
                this.actionParameters = MakeActionParametersString(filterContext.ActionParameters);

                filterContext.Controller.ViewBag.Tokens = this.autorizzazioniService.GetViewBagTokens(HttpContext.Current.Session, this.identity, controllerName, actionName);
            }

        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            swAction.Stop();
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            swResult.Restart();
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            swResult.Stop();
            Log(swResult.Elapsed, filterContext.RouteData);
        }

        private string MakeActionParametersString(IDictionary<string, object> parameters)
        {
            string str = "";

            foreach (string key in parameters.Keys)
            {
                if (parameters[key] != null)
                {
                    str += key + "=" + parameters[key].ToString() + "&";
                }
            }

            if (!String.IsNullOrEmpty(str))
            {
                str = str.Substring(0, str.Length - 1);
            }

            return str;
        }

        private void Log(TimeSpan elapsed, RouteData routeData)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var totalTime = TimeSpan.FromMilliseconds(swAction.Elapsed.TotalMilliseconds + swResult.Elapsed.TotalMilliseconds);

            var message = String.Format("{0}/{1}?{4} [{2} sec + {3} sec]", controllerName, actionName, swAction.Elapsed.ToString("s\\.f"), swResult.Elapsed.ToString("s\\.f"), this.actionParameters);

            LoggerConfig.MvcLog.Info(message);
            this.logsService.Create(new Application.Logs.Dtos.LogRecordDto()
            {
                Date = DateTime.Now,
                Thread = "service",
                Level = "INFO",
                Logger = "mvc",
                Identity = this.identity,
                Message = message
            });

            Debug.WriteLine(message, "MVC Action Filter Log");
        }


    }
}