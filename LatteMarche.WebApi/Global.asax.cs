using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using LatteMarche.WebApi.App_Start;

namespace LatteMarche.WebApi
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            WebConfig.RegisterRoutes(RouteTable.Routes);

            LoggerConfig.Configure();
            AutoFacConfig.Configure();
			AutoMapperConfig.Configure();
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
            }
        }

        private static bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(@"~/api");
        }

    }
}
