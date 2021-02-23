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
using LatteMarche.Application.Autocisterne.Interfaces;
using LatteMarche.Application.Dashboard.Services;
using LatteMarche.Application.Dashboard.Interfaces;
using LatteMarche.Application.Allevamenti.Services;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.Trasportatori.Interfaces;
using LatteMarche.Application.Trasportatori.Services;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using WeCode.Application;

namespace LatteMarche.Application
{

    /// <summary>
    /// Modulo AutoFac per la registrazione dei servizi
    /// </summary>
    public class ApplicationModule : BaseModule
    {

        public ApplicationModule(bool isWeb = true)
            : base(isWeb)
        { }

        protected override void Load(ContainerBuilder builder)
		{
            builder.RegisterModule(new AssamModule(isWeb));

            RegisterService<LatteMarcheDbContext, DbContext>(builder);
            RegisterService<UnitOfWork, IUnitOfWork>(builder);

            builder.AddAutoMapper(this.GetType().Assembly);

            RegisterService<AcquirentiService, IAcquirentiService>(builder);
            RegisterService<AllevamentiService, IAllevamentiService>(builder);
            RegisterService<AnalisiService, IAnalisiService>(builder);
            RegisterService<AutorizzazioniService, IAutorizzazioniService>(builder);
            RegisterService<AutocisterneService, IAutocisterneService>(builder);
            RegisterService<AzioniService, IAzioniService>(builder);
            RegisterService<CessionariService, ICessionariService>(builder);
            RegisterService<ComuniService, IComuniService>(builder);
            RegisterService<DestinatariService, IDestinatariService>(builder);
            RegisterService<DispositiviService, IDispositiviService>(builder);
            RegisterService<GiriService, IGiriService>(builder);
            RegisterService<LaboratoriAnalisiService, ILaboratoriAnalisiService>(builder);            
            RegisterService<LottiService, ILottiService>(builder);
            RegisterService<PrelieviLatteService, IPrelieviLatteService>(builder);
            RegisterService<RuoliService, IRuoliService>(builder);
            RegisterService<SitraService, ISitraService>(builder);
            RegisterService<SynchService, ISynchService>(builder);
            RegisterService<TipiLatteService, ITipiLatteService>(builder);
            RegisterService<TipiProfiloService, ITipiProfiloService>(builder);
            RegisterService<TrasportatoriService, ITrasportatoriService>(builder);
            RegisterService<UtentiService, IUtentiService>(builder);
            RegisterService<WidgetsService, IWidgetsService>(builder);

            RegisterService<AnalisiComparativaService, IAnalisiComparativaService>(builder);
            RegisterService<AnalisiQualitativaService, IAnalisiQualitativaService>(builder);
            RegisterService<AnalisiQuantitativaService, IAnalisiQuantitativaService>(builder);


            builder.RegisterType<LogsService>().As<ILogsService>().SingleInstance();            

            base.Load(builder);
		}

    }
}
