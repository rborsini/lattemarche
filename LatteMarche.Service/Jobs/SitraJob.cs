using System;
using System.Configuration;

namespace LatteMarche.Service.Jobs
{
    public class SitraJob : BaseJob
    {
        #region Properties


        #endregion

        #region Constructors

        public SitraJob() : base()
        {
        }

        #endregion

        #region Methods

        public override void Execute()
        {
            this.log.Debug("Debug synch job");
            this.log.Info("Info synch job");

        }


        public void SelectPrelieviSitra()
        {

        }

        #endregion
    }
}
