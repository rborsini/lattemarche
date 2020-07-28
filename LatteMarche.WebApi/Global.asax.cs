using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using LatteMarche.WebApi.App_Start;
using log4net;

namespace LatteMarche.WebApi
{
    public class Global : HttpApplication
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            WebConfig.RegisterRoutes(RouteTable.Routes);
            MobileHubConfig.Configure();
            LoggerConfig.Configure();
            AutoFacConfig.Configure();
			AutoMapperConfig.Configure();

            log.Info("Application started");
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
