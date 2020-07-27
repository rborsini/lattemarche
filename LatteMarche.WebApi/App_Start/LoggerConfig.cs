using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace LatteMarche.WebApi.App_Start
{
    public class LoggerConfig
    {

        /// <summary>
        /// Configure the log4net logger using the xml Web.config file
        /// </summary>
        public static void Configure()
        {
            // Load xml configuration
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}