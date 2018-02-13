using LatteMarche.Synch.DataType;
using LatteMarche.Synch;
using LatteMarche.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;

namespace LatteMarche.Service.Jobs
{
    public class SynchJob : BaseJob
    {
        #region Properties
        private int DepthDays { get { return Convert.ToInt32(ConfigurationManager.AppSettings["days_depth"]); } }
        private string connectionString = ConfigurationManager.ConnectionStrings["DbLatteMarcheContext"].ConnectionString;
        private string baseUrl = ConfigurationManager.AppSettings["ClientSettingsProvider.ServiceUri"];
        private bool pullBool = Convert.ToBoolean(ConfigurationManager.AppSettings["Pull"]);
        private bool pushBool = Convert.ToBoolean(ConfigurationManager.AppSettings["Push"]);
        #endregion

        #region Constructors

        public SynchJob(): base()
        {
        }

        #endregion

        #region Methods

        public override void Execute()
        {
            this.log.Debug("Debug synch job");
            this.log.Info("Info synch job");
            Synch.Service synch = new Synch.Service(connectionString, DepthDays, baseUrl);
            if (pullBool) synch.Pull();
            if (pushBool) synch.Push();
        }


        #endregion
    }
}
