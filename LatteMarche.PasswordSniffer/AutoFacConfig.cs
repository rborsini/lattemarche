using Autofac;
using LatteMarche.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.PasswordSniffer
{
    public class AutoFacConfig
    {
        internal static IContainer Container;

        internal static void Configure()
        {
            ContainerBuilder builder = new ContainerBuilder();

            // Registro i db context e unit of work
            builder.RegisterModule(new ApplicationModule(false));

            Container = builder.Build();


        }
    }
}
