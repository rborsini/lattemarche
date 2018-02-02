using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LatteMarche.Service.Jobs
{
    public class SynchJob : BaseJob
    {
        #region Properties

        #endregion

        #region Constructors

        public SynchJob()
            : base() { }

        #endregion

        #region Methods

        public override void Execute()
        {
            this.log.Debug("Debug synch job");
            this.log.Info("Info synch job");

            System.Threading.Thread.Sleep(6000);


        }

        #endregion
    }
}
