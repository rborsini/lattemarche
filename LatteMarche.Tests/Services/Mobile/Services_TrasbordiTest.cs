using Autofac;
using Bogus;
using FluentAssertions;
using LatteMarche.Application.Autocisterne.Interfaces;
using LatteMarche.Application.Dispositivi.Dtos;
using LatteMarche.Application.Dispositivi.Interfaces;
using LatteMarche.Application.Mobile.Dtos;
using LatteMarche.Application.Mobile.Interfaces;
using LatteMarche.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Tests.Services.Mobile
{
    [TestFixture]
    public class Services_TrasbordiTest
    {
        #region Constants

        private const string IMEI_ORIGINE = "1111";
        private const string TARGA_ORIGINE = "EA-005-NE";

        private const string IMEI_DESTINAZIONE = "2222";
        private const string TARGA_DESTINAZIONE = "FA-259-AD";

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<Trasbordo, long> trasbordiRepository;

        private IAutocisterneService autocisterneService;
        private IDispositiviService dispositiviService;
        private ITrasbordiService trasbordiService;

        private DbCleaner dbCleaner;

        private Faker<DispositivoMobileDto> dispositiviFaker;
        private Faker<PrelievoLatteDto> prelieviFaker;
        private Faker<TrasbordoDto> trabordiFaker;

        #endregion

        #region Constructor

        public Services_TrasbordiTest()
        {
            AutoFacConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();
            this.dbCleaner = new DbCleaner(uow);

            this.trasbordiRepository = this.uow.Get<Trasbordo, long>();

            this.autocisterneService = this.scope.Resolve<IAutocisterneService>();
            this.dispositiviService = this.scope.Resolve<IDispositiviService>();
            this.trasbordiService = this.scope.Resolve<ITrasbordiService>();
        }

        #endregion

        #region Init / Clean Methods

        [SetUp]
        public void Init()
        {
            var autocisterna_origine = this.autocisterneService.Create(new Application.Autocisterne.Dtos.AutocisternaDto() { Targa = TARGA_ORIGINE });
            var autocisterna_destinazione = this.autocisterneService.Create(new Application.Autocisterne.Dtos.AutocisternaDto() { Targa = TARGA_DESTINAZIONE });

            this.dispositiviFaker = new Faker<DispositivoMobileDto>("it")
                .RuleFor(f => f.Attivo, f => true)
                .RuleFor(f => f.DataRegistrazione, f => DateTime.UtcNow)
                .RuleFor(f => f.DataUltimoDownload, f => DateTime.UtcNow)
                .RuleFor(f => f.DataUltimoUpload, f => DateTime.UtcNow)
                .RuleSet("origine", (set) =>
                {
                    set.RuleFor(f => f.Id, f => IMEI_ORIGINE);
                    set.RuleFor(f => f.IdAutocisterna, f => autocisterna_origine.Id);
                })
                .RuleSet("destinazione", (set) =>
                {
                    set.RuleFor(f => f.Id, f => IMEI_DESTINAZIONE);
                    set.RuleFor(f => f.IdAutocisterna, f => autocisterna_destinazione.Id);
                })
                ;

            var dispositivoOrigine = this.dispositiviFaker.Generate("default,origine");
            var dispositivoDestinazione = this.dispositiviFaker.Generate("default,destinazione");

            this.dispositiviService.Create(dispositivoOrigine);
            this.dispositiviService.Create(dispositivoDestinazione);

            this.prelieviFaker = new Faker<PrelievoLatteDto>("it")
                .RuleFor(f => f.DataPrelievo, f => DateTime.Now)
                .RuleFor(f => f.DataUltimaMungitura, f => DateTime.Today)
                .RuleFor(f => f.IdAcquirente, f => 1)
                .RuleFor(f => f.IdAllevamento, f => 1)
                .RuleFor(f => f.IdAutocisterna, f => 1)
                .RuleFor(f => f.IdDestinatario, f => 1)
                .RuleFor(f => f.IdGiro, f => 1)
                .RuleFor(f => f.IdTrasportatore, f => 1)
                .RuleFor(f => f.Lat, f => Convert.ToDecimal(f.Address.Latitude()))
                .RuleFor(f => f.Lng, f => Convert.ToDecimal(f.Address.Longitude()))
                .RuleFor(f => f.NumeroMungiture, f => 1)
                .RuleFor(f => f.Quantita, f => 111)
                .RuleFor(f => f.Scomparto, f => "2-3")
                .RuleFor(f => f.Temperatura, f => 2)
                ;

            this.trabordiFaker = new Faker<TrasbordoDto>("it")
                .RuleFor(f => f.IMEI_Origine, f => f.Hacker.Abbreviation())
                .RuleFor(f => f.Targa_Origine, f => "EA-005-NE")
                .RuleFor(f => f.IMEI_Destinazione, f => f.Hacker.Abbreviation())
                .RuleFor(f => f.Targa_Destinazione, f => TARGA_DESTINAZIONE)
                .RuleFor(f => f.Data, f => DateTime.Now)
                .RuleFor(f => f.Lat, f => Convert.ToDecimal(f.Address.Latitude()))
                .RuleFor(f => f.Lng, f => Convert.ToDecimal(f.Address.Longitude()))
                .RuleFor(f => f.IdTemplateGiro, f => 1)
                .RuleFor(f => f.Prelievi, f => this.prelieviFaker.Generate(3))
                ;
        }

        [TearDown]
        public void CleanUp()
        {
            this.dbCleaner.CleanUp();
        }

        #endregion

        #region Test Methods

        [Test]
        public void TrasbordiService_Push()
        {
            var trasbordoDto = this.trabordiFaker.Generate();
            trasbordoDto = this.trasbordiService.Push(trasbordoDto);
            trasbordoDto.Should().NotBeNull();

            var trasbordo = this.trasbordiRepository.GetById(trasbordoDto.Id);
            trasbordo.Targa_Destinazione.Should().Be(TARGA_DESTINAZIONE);

        }

        [Test]
        public void TrasbordiService_Pull()
        {
            var trasbordoDto = this.trabordiFaker.Generate();
            trasbordoDto = this.trasbordiService.Push(trasbordoDto);

            var trasbordi = this.trasbordiService.Pull(trasbordoDto.IMEI_Destinazione);
            trasbordi.Should().HaveCount(1);
        }

        [Test]
        public void TrasbordiService_Close()
        {
            var trasbordoDto = this.trabordiFaker.Generate();
            trasbordoDto = this.trasbordiService.Push(trasbordoDto);

            this.trasbordiService.Close(trasbordoDto.Id);

            var trasbordo = this.trasbordiRepository.GetById(trasbordoDto.Id);
            trasbordo.Chiuso.Should().Be(true);
        }


        #endregion
    }
}
