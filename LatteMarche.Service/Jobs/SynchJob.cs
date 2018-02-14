using System;
using System.Configuration;

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
            //Synch.SynchService synch = new Synch.SynchService(connectionString, DepthDays, baseUrl, this.log);
            //if (pullBool) synch.Pull();
            //if (pushBool) synch.Push();
        }


        #endregion
    }
}
