using System.Web.Mvc;
using System.Web.Routing;

namespace LatteMarche.WebApi.App_Start
{
    public class WebConfig
    {
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		} 
    }

}
