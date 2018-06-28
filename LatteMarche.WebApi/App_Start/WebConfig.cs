using System.Web.Mvc;
using System.Web.Routing;

namespace LatteMarche.WebApi.App_Start
{
    public class WebConfig
    {
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //https://code.i-harness.com/it/q/1071ec7
            routes.LowercaseUrls = true;

            routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		} 
    }

}
