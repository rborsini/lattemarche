using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net.Http;
using LatteMarche.Application.Auth.Interfaces;
using System.Configuration;
using System;
using log4net;
using System.Diagnostics;

namespace LatteMarche.WebApi.Filters
{
    public class ApiCustomAuthorize : AuthorizeAttribute
    {
        private bool apiAuthEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["apiAuthEnabled"]);

        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = actionContext.ActionDescriptor.ActionName;
            string userName = HttpContext.Current.User.Identity.Name;

            ////http://docs.autofac.org/en/latest/integration/webapi.html#register-the-filter-provider
            //// Standard Web API Filters are Singletons
            var requestScope = actionContext.ControllerContext.Request.GetDependencyScope();

            IAutorizzazioniService service = requestScope.GetService(typeof(IAutorizzazioniService)) as IAutorizzazioniService;

            ////https://soabubblog.wordpress.com/2013/07/10/web-api-sessions/
            var authorized = apiAuthEnabled ? service.Authorize(HttpContext.Current.Session, userName, "API", controllerName, actionName) : true;

            sw.Stop();
            log.Debug($"API Authorization [Controller: {controllerName} - Action: {actionName} - Time: {sw.Elapsed}]");
            
            return authorized;

        }
    }
}