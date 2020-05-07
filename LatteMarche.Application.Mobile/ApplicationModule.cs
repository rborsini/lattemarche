using Autofac;
using LatteMarche.Application.Mobile.Interfaces;
using LatteMarche.Application.Mobile.Services;
using LatteMarche.Core;
using LatteMarche.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data;
using WeCode.Data.Interfaces;

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
            RegisterService<LatteMarcheDbContext, DbContext>(builder);
            RegisterService<UnitOfWork, IUnitOfWork>(builder);

            RegisterService<MobileService, IMobileService>(builder);

            base.Load(builder);
        }

        private void RegisterService<TService, TInterface>(ContainerBuilder builder)
        {
            var registration = builder.RegisterType<TService>().As<TInterface>();

            if (this.isWeb)
                registration.InstancePerRequest();
        }

    }
}
