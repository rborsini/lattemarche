using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace LatteMarche.WebApi.App_Start
{
    public class LoggerConfig
    {

        public static Utils.Logs.Logger ApiLog { get; private set; }
        public static Utils.Logs.Logger MvcLog { get; private set; }

        /// <summary>
        /// Configure the log4net logger using the xml Web.config file
        /// </summary>
        public static void Configure()
        {
            // Load xml configuration
            log4net.Config.XmlConfigurator.Configure();

            // Get loggers
            ApiLog = new Utils.Logs.Logger(LogManager.GetLogger("api"));
            MvcLog = new Utils.Logs.Logger(LogManager.GetLogger("mvc"));

            // Log Start
            ApiLog.Info("Application started");
            MvcLog.Info("Application started");
        }
    }
}