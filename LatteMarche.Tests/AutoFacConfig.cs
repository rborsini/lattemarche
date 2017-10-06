using Autofac;
using LatteMarche.Application;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.Allevamenti.Services;
using LatteMarche.Application.Allevatori.Interfaces;
using LatteMarche.Application.Allevatori.Services;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Application.Comuni.Services;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.Application.Giri.Services;
using LatteMarche.Application.TipiLatte.Interfaces;
using LatteMarche.Application.TipiLatte.Services;
using LatteMarche.Application.TipiProfilo.Interfaces;
using LatteMarche.Application.TipiProfilo.Services;
using LatteMarche.Application.Trasportatori.Interfaces;
using LatteMarche.Application.Trasportatori.Services;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Application.Utenti.Services;
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

            builder.RegisterType<InMemoryContext>().As<IContext>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterType<UtentiService>().As<IUtentiService>();
            builder.RegisterType<ComuniService>().As<IComuniService>();
            builder.RegisterType<TipiLatteService>().As<ITipiLatteService>();
            builder.RegisterType<TipiProfiloService>().As<ITipiProfiloService>();
            builder.RegisterType<AllevatoriService>().As<IAllevatoriService>();
            builder.RegisterType<AllevamentiService>().As<IAllevamentiService>();
            builder.RegisterType<TrasportatoriService>().As<ITrasportatoriService>();
            builder.RegisterType<GiriService>().As<IGiriService>();

            Container = builder.Build();
        }

    }
}
