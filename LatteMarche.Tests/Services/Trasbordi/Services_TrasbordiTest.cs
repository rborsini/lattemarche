using Autofac;
using Bogus;
using FluentAssertions;
using LatteMarche.Application.Trasbordi.Dtos;
using LatteMarche.Application.Trasbordi.Interfaces;
using LatteMarche.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Tests.Services.Trasbordi
{
    [TestFixture]
    public class Services_TrasbordiTest
    {
        #region Constants

        private const int ID_PROFILO_TRASPORTATORE = 5;
        private const string CODICE_GIRO = "L1";
        private const string DENOMINAZIONE_GIRO = "Latte 1";

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<Utente, int> utentiRepository;
        private IRepository<Trasbordo, long> trasbordiRepository;
        private IRepository<Giro, int> giriRepository;

        private ITrasbordiService trasbordiService;

        private DbCleaner dbCleaner;

        private Faker<Utente> utentiFaker;
        private Faker<Giro> giriFaker;
        private Faker<Trasbordo> trabordiFaker;

        #endregion

        #region Constructor
        public Services_TrasbordiTest()
        {
            AutoFacConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();
            this.dbCleaner = new DbCleaner(uow);

            this.utentiRepository = this.uow.Get<Utente, int>();
            this.trasbordiRepository = this.uow.Get<Trasbordo, long>();
            this.giriRepository = this.uow.Get<Giro, int>();

            this.trasbordiService = this.scope.Resolve<ITrasbordiService>();
        }

        #endregion

        #region Init / Clean Methods

        [SetUp]
        public void Init()
        {
            // trasportatore
            this.utentiFaker = new Faker<Utente>("it")
                .RuleFor(f => f.IdProfilo, f => ID_PROFILO_TRASPORTATORE);

            var trasportatore = this.utentiFaker.Generate();
            this.utentiRepository.Add(trasportatore);
            this.uow.SaveChanges();

            // giri
            this.giriFaker = new Faker<Giro>("it")
                .RuleFor(f => f.CodiceGiro, f => CODICE_GIRO)
                .RuleFor(f => f.Denominazione, f => DENOMINAZIONE_GIRO)
                .RuleFor(f => f.IdTrasportatore, f => trasportatore.Id)
                ;

            var giro = this.giriFaker.Generate();
            this.giriRepository.Add(giro);
            this.uow.SaveChanges();

            // trasbordi
            this.trabordiFaker = new Faker<Trasbordo>("it")
                .RuleFor(f => f.IMEI_Origine, f => f.Hacker.Abbreviation())
                .RuleFor(f => f.Targa_Origine, f => "EA-005-NE")
                .RuleFor(f => f.IMEI_Destinazione, f => f.Hacker.Abbreviation())
                .RuleFor(f => f.Targa_Destinazione, f => "FA-259-AD")                
                .RuleFor(f => f.Lat, f => Convert.ToDouble(f.Address.Latitude()))
                .RuleFor(f => f.Lng, f => Convert.ToDouble(f.Address.Longitude()))
                .RuleFor(f => f.IdTemplateGiro, f => giro.Id)
                .RuleSet("today", (set) =>
                {
                    set.RuleFor(f => f.Data, f => DateTime.Now);
                })

                .RuleSet("yesterday", (set) =>
                {
                    set.RuleFor(f => f.Data, f => DateTime.Now.AddDays(-1));
                })
                ;

            var trasbordi = this.trabordiFaker.Generate(10, "default, today");
            this.trasbordiRepository.Add(trasbordi);
            this.uow.SaveChanges();

            trasbordi = this.trabordiFaker.Generate(10, "default, yesterday");
            this.trasbordiRepository.Add(trasbordi);
            this.uow.SaveChanges();

        }

        [TearDown]
        public void CleanUp()
        {
            this.dbCleaner.CleanUp();
        }

        #endregion

        #region Test Methods

        [Test]
        public void TrasbordiService_Search()
        {
            var trasbordi = this.trasbordiService.Search(new TrasbordiSearchDto()
            {
                DataInizio = DateTime.Today
            });

            trasbordi.Should().HaveCount(10);
        }

        [Test]
        public void TrasbordiService_Details()
        {
            var trasbordi = this.trasbordiService.Search(null);

            var trasbordo = this.trasbordiService.Details(trasbordi[0].Id);
            trasbordo.Should().NotBeNull();

            trasbordo.DenominazioneGiro.Should().Be(DENOMINAZIONE_GIRO);
        }

        #endregion

    }
}
