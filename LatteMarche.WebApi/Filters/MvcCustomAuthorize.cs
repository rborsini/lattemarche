using LatteMarche.Application.Auth.Interfaces;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace LatteMarche.WebApi.Filters
{
    public class MvcCustomAuthorize : AuthorizeAttribute
    {
        // http://docs.autofac.org/en/latest/integration/mvc.html
        public IAutorizzazioniService service { get; set; }

        private bool mvcAuthEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["mvcAuthEnabled"]);

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var rd = httpContext.Request.RequestContext.RouteData;

            string controllerName = rd.GetRequiredString("controller");
            string actionName = rd.GetRequiredString("action");
            string userName = httpContext.User.Identity.Name;


            //return httpContext.User.Identity.IsAuthenticated;
            return mvcAuthEnabled ? service.Authorize(HttpContext.Current.Session, userName, "MVC", controllerName, actionName) : true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //you can change to any controller or html page.
            filterContext.Result = new RedirectResult("~/home/unauthorized");

        }

    }
}