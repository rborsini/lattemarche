using LatteMarche.Application.PrelieviLatte.Dtos;
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
        #endregion

        #region Constructors

        public SynchJob(): base()
        {
        }

        #endregion

        #region Methods

        public void synchronizator()
        {
            LatteMarche.Synch.Service synch = new Synch.Service(connectionString, DepthDays, baseUrl);
            List<PrelievoLatteDto> prelievi = synch.Pull();
            synch.Push(prelievi);
        }

        public override void Execute()
        {
            this.log.Debug("Debug synch job");
            this.log.Info("Info synch job");

            System.Threading.Thread.Sleep(6000);


        }


        #endregion
    }
}
