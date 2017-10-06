using Autofac;
using LatteMarche.Application;
using LatteMarche.Core;
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

            // Db Context
            builder.RegisterType<InMemoryContext>().As<IContext>().InstancePerLifetimeScope();

            // Unit of Work
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterModule(new ApplicationModule());

            Container = builder.Build();
        }

    }
}
