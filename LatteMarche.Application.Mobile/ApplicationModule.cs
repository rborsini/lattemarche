using Autofac;
using LatteMarche.Application.Mobile.Interfaces;
using LatteMarche.Application.Mobile.Services;
using LatteMarche.Core;
using LatteMarche.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Mobile
{
    public class ApplicationModule : Module
    {
        private bool isWeb;

        public ApplicationModule(bool isWeb = true)
        {
            this.isWeb = isWeb;
        }

        protected override void Load(ContainerBuilder builder)
        {

            if (this.isWeb)
            {
                builder.RegisterModule(new DataModule());

                builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();

                builder.RegisterType<MobileService>().As<IMobileService>().InstancePerRequest();

            }
            else
            {
                builder.RegisterModule(new DataModule(false));

                builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

                builder.RegisterType<MobileService>().As<IMobileService>();
            }


            base.Load(builder);
        }
    }
}
