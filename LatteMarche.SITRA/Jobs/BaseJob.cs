using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.SITRA.Jobs
{
    [DisallowConcurrentExecution]
    public abstract class BaseJob : IJob
    {
        #region Fields

        protected ILog log;
        protected log4net.ILog serviceLog = log4net.LogManager.GetLogger("Service");
        private Stopwatch sw;

        #endregion

        #region Properties

        protected Dictionary<string, string> Params;

        protected string Name { get { return this.Params["name"]; } }
        protected string Logger { get { return this.Params["name"]; } }


        #endregion

        #region Constructors

        public BaseJob()
        {
            this.Params = new Dictionary<string, string>();
            this.sw = new Stopwatch();

        }

        #endregion

        #region Methods

        public virtual void Execute(IJobExecutionContext context)
        {
            LoadParameters(context.JobDetail.JobDataMap);
            log = LogManager.GetLogger(this.Logger);

            serviceLog.Debug(String.Format("{0} job avviato...", this.Name));
            log.Debug(String.Format("{0} job avviato...", this.Name));

            sw.Restart();

            try
            {
                Execute();
            }
            catch (Exception exc)
            {
                log.Error("Errore imprevisto", exc);
            }

            sw.Stop();

            serviceLog.Debug(String.Format("{0} job eseguito in {1}.", this.Name, sw.Elapsed));
            log.Debug(String.Format("{0} job eseguito in {1}.", this.Name, sw.Elapsed));
        }

        public abstract void Execute();

        private void LoadParameters(JobDataMap dataMap)
        {
            this.Params.Clear();
            foreach (string key in dataMap.Keys)
            {
                this.Params.Add(key, dataMap.GetString(key));
            }
        }

        #endregion
    }
}