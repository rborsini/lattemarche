using System;
using System.Configuration;

namespace LatteMarche.SITRA.Jobs
{
    public class SynchJob : BaseJob
    {
        #region Properties


        #endregion

        #region Constructors

        public SynchJob() : base()
        {
        }

        #endregion

        #region Methods

        public override void Execute()
        {
            this.log.Debug("Debug synch job");
            this.log.Info("Info synch job");


        }


        #endregion
    }
}