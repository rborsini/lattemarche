using LatteMarche.Application.Auth.Interfaces;
using System.Web;
using System.Web.Mvc;

namespace LatteMarche.WebApi.Attributes
{
    public class MvcCustomAuthorize : AuthorizeAttribute
    {
        // http://docs.autofac.org/en/latest/integration/mvc.html
        public IAutorizzazioniService service { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var rd = httpContext.Request.RequestContext.RouteData;

            string controllerName = rd.GetRequiredString("controller");
            string actionName = rd.GetRequiredString("action");
            string userName = httpContext.User.Identity.Name;


            //return httpContext.User.Identity.IsAuthenticated;
            return service.Authorize(HttpContext.Current.Session, userName, "MVC", controllerName, actionName);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //you can change to any controller or html page.
            filterContext.Result = new RedirectResult("~/home/unauthorized");

        }

    }
}