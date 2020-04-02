using Autofac;
using LatteMarche.Application.Assam.Interfaces;
using LatteMarche.Application.Assam.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Assam
{
    public class AssamModule : Module
    {
        private bool isWeb;

        public AssamModule(bool isWeb = true)
        {
            this.isWeb = isWeb;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (this.isWeb)
            {
                builder.RegisterType<AssamService>().As<IAssamService>().InstancePerRequest();
            }
            else
            {
                builder.RegisterType<AssamService>().As<IAssamService>();
            }


            base.Load(builder);
        }
    }
}
