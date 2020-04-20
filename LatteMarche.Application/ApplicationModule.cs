using Autofac;
using LatteMarche.EntityFramework;
using LatteMarche.Core;
using LatteMarche.Application.Comuni.Services;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Application.Allevamenti.Services;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.Trasportatori.Services;
using LatteMarche.Application.Trasportatori.Interfaces;
using LatteMarche.Application.Documenti.Interfaces;
using LatteMarche.Application.Documenti.Services;
using LatteMarche.Application.Acquirenti.Services;
using LatteMarche.Application.Acquirenti.Interfaces;
using LatteMarche.Application.Destinatari.Services;
using LatteMarche.Application.Destinatari.Interfaces;
using LatteMarche.Application.Synch.Services;
using LatteMarche.Application.Synch.Interfaces;
using LatteMarche.Application.Sitra.Services;
using LatteMarche.Application.Sitra.Interfaces;
using LatteMarche.Application.Logs.Interfaces;
using LatteMarche.Application.Logs.Services;
using LatteMarche.Application.Assam;
using LatteMarche.Application.AnalisiLatte.Interfaces;
using LatteMarche.Application.AnalisiLatte.Services;
using LatteMarche.Application.Mobile.Interfaces;
using LatteMarche.Application.Mobile.Services;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Application.Latte.Interfaces;
using LatteMarche.Application.Auth.Services;
using LatteMarche.Application.Latte.Services;

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

			if (this.isWeb)
			{
				builder.RegisterModule(new DataModule());
                builder.RegisterModule(new AssamModule());

                builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();

                builder.RegisterType<AutocisterneService>().As<IAutocisterneService>().InstancePerRequest();
                builder.RegisterType<AcquirentiService>().As<IAcquirentiService>().InstancePerRequest();
                builder.RegisterType<AllevamentiService>().As<IAllevamentiService>().InstancePerRequest();
                builder.RegisterType<AllevatoriService>().As<IAllevatoriService>().InstancePerRequest();
                builder.RegisterType<AnalisiService>().As<IAnalisiService>().InstancePerRequest();
                builder.RegisterType<AutorizzazioniService>().As<IAutorizzazioniService>().InstancePerRequest();
                builder.RegisterType<AzioniService>().As<IAzioniService>().InstancePerRequest();
                builder.RegisterType<ComuniService>().As<IComuniService>().InstancePerRequest();
                builder.RegisterType<DestinatariService>().As<IDestinatariService>().InstancePerRequest();
                builder.RegisterType<DocumentiService>().As<IDocumentiService>().InstancePerRequest();
                builder.RegisterType<GiriService>().As<IGiriService>().InstancePerRequest();
                builder.RegisterType<LaboratoriAnalisiService>().As<ILaboratoriAnalisiService>().InstancePerRequest();
                builder.RegisterType<LogsService>().As<ILogsService>().InstancePerRequest();
                builder.RegisterType<LottiService>().As<ILottiService>().InstancePerRequest();
                builder.RegisterType<PrelieviLatteService>().As<IPrelieviLatteService>().InstancePerRequest();
                builder.RegisterType<RuoliService>().As<IRuoliService>().InstancePerRequest();
                builder.RegisterType<SitraService>().As<ISitraService>().InstancePerRequest();
                builder.RegisterType<SynchService>().As<ISynchService>().InstancePerRequest();
                builder.RegisterType<TipiLatteService>().As<ITipiLatteService>().InstancePerRequest();
                builder.RegisterType<TipiProfiloService>().As<ITipiProfiloService>().InstancePerRequest();
                builder.RegisterType<TrasportatoriService>().As<ITrasportatoriService>().InstancePerRequest();
                builder.RegisterType<UtentiService>().As<IUtentiService>().InstancePerRequest();
                builder.RegisterType<MobileService>().As<IMobileService>().InstancePerRequest();

            }
            else
			{
				builder.RegisterModule(new DataModule(false));

				builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

                builder.RegisterType<AutocisterneService>().As<IAutocisterneService>();
                builder.RegisterType<AcquirentiService>().As<IAcquirentiService>();
                builder.RegisterType<AllevamentiService>().As<IAllevamentiService>();
                builder.RegisterType<AllevatoriService>().As<IAllevatoriService>();
                builder.RegisterType<AnalisiService>().As<IAnalisiService>();
                builder.RegisterType<AutorizzazioniService>().As<IAutorizzazioniService>();
                builder.RegisterType<AzioniService>().As<IAzioniService>();
                builder.RegisterType<ComuniService>().As<IComuniService>();
                builder.RegisterType<DestinatariService>().As<IDestinatariService>();
                builder.RegisterType<DocumentiService>().As<IDocumentiService>();
                builder.RegisterType<GiriService>().As<IGiriService>();
                builder.RegisterType<LaboratoriAnalisiService>().As<ILaboratoriAnalisiService>();
                builder.RegisterType<LogsService>().As<ILogsService>();
                builder.RegisterType<LottiService>().As<ILottiService>();
                builder.RegisterType<PrelieviLatteService>().As<IPrelieviLatteService>();
                builder.RegisterType<RuoliService>().As<IRuoliService>();
                builder.RegisterType<SitraService>().As<ISitraService>();
                builder.RegisterType<SynchService>().As<ISynchService>();
                builder.RegisterType<TipiLatteService>().As<ITipiLatteService>();
                builder.RegisterType<TipiProfiloService>().As<ITipiProfiloService>();
                builder.RegisterType<TrasportatoriService>().As<ITrasportatoriService>();
                builder.RegisterType<UtentiService>().As<IUtentiService>();
                builder.RegisterType<MobileService>().As<IMobileService>();
            }


            base.Load(builder);
		}
	}
}
