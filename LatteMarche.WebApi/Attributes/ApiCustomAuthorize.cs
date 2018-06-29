using LatteMarche.Application.Ruoli.Interfaces;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net.Http;

namespace LatteMarche.WebApi.Attributes
{
    public class ApiCustomAuthorize : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            string controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = actionContext.ActionDescriptor.ActionName;
            string userName = HttpContext.Current.User.Identity.Name;

            ////http://docs.autofac.org/en/latest/integration/webapi.html#register-the-filter-provider
            //// Standard Web API Filters are Singletons
            var requestScope = actionContext.ControllerContext.Request.GetDependencyScope();

            IAutorizzazioniService service = requestScope.GetService(typeof(IAutorizzazioniService)) as IAutorizzazioniService;

            ////https://soabubblog.wordpress.com/2013/07/10/web-api-sessions/

            return service.Authorize(HttpContext.Current.Session, userName, "API", controllerName, actionName);

            //return HttpContext.Current.User.Identity.IsAuthenticated;
        }
    }
}