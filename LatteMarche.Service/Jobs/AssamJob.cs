using Autofac;
using LatteMarche.Application.AnalisiLatte.Interfaces;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Service.Jobs
{
    public class AssamJob : BaseJob
    {
        #region Constructor

        public AssamJob()
            : base() { }

        #endregion

        #region Methods
        public override void Execute()
        {
            using (ILifetimeScope scope = AutoFacConfig.Container.BeginLifetimeScope())
            {
                var analisiService = scope.Resolve<IAnalisiService>();
                analisiService.Synch();
                

                Console.WriteLine("FATTO");
            }
        }

        #endregion

    }
}
