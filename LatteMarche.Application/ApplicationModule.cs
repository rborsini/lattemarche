using Autofac;
using LatteMarche.EntityFramework;
using LatteMarche.Core;
using LatteMarche.Application.Utenti.Services;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Application.Comuni.Services;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Application.TipiLatte.Services;
using LatteMarche.Application.TipiLatte.Interfaces;
using LatteMarche.Application.TipiProfilo.Services;
using LatteMarche.Application.TipiProfilo.Interfaces;
using LatteMarche.Application.Allevatori.Services;
using LatteMarche.Application.Allevatori.Interfaces;
using LatteMarche.Application.Allevamenti.Services;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.Trasportatori.Services;
using LatteMarche.Application.Trasportatori.Interfaces;
using LatteMarche.Application.Giri.Services;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.Application.PrelieviLatte.Services;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Application.LaboratoriAnalisi.Services;
using LatteMarche.Application.LaboratoriAnalisi.Interfaces;
using LatteMarche.Application.Acquirenti.Services;
using LatteMarche.Application.Acquirenti.Interfaces;


namespace LatteMarche.Application
{

    /// <summary>
    /// Modulo AutoFac per la registrazione dei servizi
    /// </summary>
    public class ApplicationModule : Module
	{
		private bool isWeb;

		public ApplicationModule(bool isWeb = true)
		{
			this.isWeb = isWeb;
		}

		protected override void Load(ContainerBuilder builder)
		{
			AutomapperConfig.Configure();

			if (this.isWeb)
			{
				builder.RegisterModule(new DataModule());

				builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();

				builder.RegisterType<UtentiService>().As<IUtentiService>().InstancePerRequest();
                builder.RegisterType<ComuniService>().As<IComuniService>().InstancePerRequest();
                builder.RegisterType<TipiLatteService>().As<ITipiLatteService>().InstancePerRequest();
                builder.RegisterType<TipiProfiloService>().As<ITipiProfiloService>().InstancePerRequest();
                builder.RegisterType<AllevatoriService>().As<IAllevatoriService>().InstancePerRequest();
                builder.RegisterType<AllevamentiService>().As<IAllevamentiService>().InstancePerRequest();
                builder.RegisterType<TrasportatoriService>().As<ITrasportatoriService>().InstancePerRequest();
                builder.RegisterType<GiriService>().As<IGiriService>().InstancePerRequest();
                builder.RegisterType<PrelieviLatteService>().As<IPrelieviLatteService>().InstancePerRequest();
                builder.RegisterType<LaboratoriAnalisiService>().As<ILaboratoriAnalisiService>().InstancePerRequest();
                builder.RegisterType<AcquirentiService>().As<IAcquirentiService>().InstancePerRequest();

            }
            else
			{
				builder.RegisterModule(new DataModule(false));

				builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

				builder.RegisterType<UtentiService>().As<IUtentiService>();
                builder.RegisterType<ComuniService>().As<IComuniService>();
                builder.RegisterType<TipiLatteService>().As<ITipiLatteService>();
                builder.RegisterType<TipiProfiloService>().As<ITipiProfiloService>();
                builder.RegisterType<AllevatoriService>().As<IAllevatoriService>();
                builder.RegisterType<AllevamentiService>().As<IAllevamentiService>();
                builder.RegisterType<TrasportatoriService>().As<ITrasportatoriService>();
                builder.RegisterType<GiriService>().As<IGiriService>();
                builder.RegisterType<PrelieviLatteService>().As<IPrelieviLatteService>();
                builder.RegisterType<LaboratoriAnalisiService>().As<ILaboratoriAnalisiService>();
                builder.RegisterType<AcquirentiService>().As<IAcquirentiService>();
            }


            base.Load(builder);
		}
	}
}
