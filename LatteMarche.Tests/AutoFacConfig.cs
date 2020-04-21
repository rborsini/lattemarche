using Autofac;
using LatteMarche.Application;
using LatteMarche.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Tests
{
    public class AutoFacConfig
    {
        public static IContainer Container { get; set; }

        internal static void Configure()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterModule(new ApplicationModule(false));
            builder.RegisterModule(new LatteMarche.Application.Mobile.ApplicationModule(false));

            Container = builder.Build();
        }

    }
}
