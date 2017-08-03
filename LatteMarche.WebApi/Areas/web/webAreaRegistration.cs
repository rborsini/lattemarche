using System.Web.Mvc;

namespace LatteMarche.WebApi.Areas.web
{
	public class webAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "web";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"web_default",
				"web/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
