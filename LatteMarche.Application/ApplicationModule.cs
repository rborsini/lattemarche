using Autofac;
using LatteMarche.EntityFramework;
using LatteMarche.Application.Comuni.Services;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Application.Synch.Services;
using LatteMarche.Application.Synch.Interfaces;
using LatteMarche.Application.Sitra.Services;
using LatteMarche.Application.Sitra.Interfaces;
using LatteMarche.Application.Logs.Interfaces;
using LatteMarche.Application.Logs.Services;
using LatteMarche.Application.Assam;
using LatteMarche.Application.AnalisiLatte.Interfaces;
using LatteMarche.Application.AnalisiLatte.Services;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Application.Auth.Services;
using LatteMarche.Application.Latte.Services;
using LatteMarche.Application.Dispositivi.Interfaces;
using LatteMarche.Application.Dispositivi.Services;
using WeCode.Data;
using WeCode.Data.Interfaces;
using System.Data.Entity;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.Application.Giri.Services;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Application.PrelieviLatte.Services;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Application.Utenti.Services;
using LatteMarche.Application.Acquirenti.Services;
using LatteMarche.Application.Acquirenti.Interfaces;
using LatteMarche.Application.Destinatari.Interfaces;
using LatteMarche.Application.Destinatari.Services;
using LatteMarche.Application.Cessionari.Services;
using LatteMarche.Application.Cessionari.Interfaces;
using LatteMarche.Application.AziendeTrasportatori.Interfaces;
using LatteMarche.Application.Autocisterne.Interfaces;

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
            builder.RegisterModule(new AssamModule());

            RegisterService<LatteMarcheDbContext, DbContext>(builder);
            RegisterService<UnitOfWork, IUnitOfWork>(builder);

            //RegisterService<AutocisterneService, IAutocisterneService>(builder);
            RegisterService<AcquirentiService, IAcquirentiService>(builder);
            //RegisterService<AllevamentiService, IAllevamentiService>(builder);
            //RegisterService<AllevatoriService, IAllevatoriService>(builder);
            RegisterService<AnalisiService, IAnalisiService>(builder);
            RegisterService<AutorizzazioniService, IAutorizzazioniService>(builder);
            RegisterService<AutocisterneService, IAutocisterneService>(builder);
            RegisterService<AziendeTrasportatoriService, IAziendeTrasportatoriService>(builder);
            RegisterService<AzioniService, IAzioniService>(builder);
            RegisterService<CessionariService, ICessionariService>(builder);
            RegisterService<ComuniService, IComuniService>(builder);
            RegisterService<DestinatariService, IDestinatariService>(builder);
            RegisterService<DispositiviService, IDispositiviService>(builder);
            RegisterService<GiriService, IGiriService>(builder);
            RegisterService<LaboratoriAnalisiService, ILaboratoriAnalisiService>(builder);
            RegisterService<LogsService, ILogsService>(builder);
            RegisterService<LottiService, ILottiService>(builder);
            RegisterService<PrelieviLatteService, IPrelieviLatteService>(builder);
            RegisterService<RuoliService, IRuoliService>(builder);
            RegisterService<SitraService, ISitraService>(builder);
            RegisterService<SynchService, ISynchService>(builder);
            RegisterService<TipiLatteService, ITipiLatteService>(builder);
            RegisterService<TipiProfiloService, ITipiProfiloService>(builder);
            //RegisterService<TrasportatoriService, ITrasportatoriService>(builder);
            RegisterService<UtentiService, IUtentiService>(builder);

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
