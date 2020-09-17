using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LatteMarche.WebApi.Helpers
{
    public static class EnvHelper
    {
        public static bool IsProd
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["frontEndEnv"] == "prod";  }
        }
    }
}