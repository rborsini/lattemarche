using Autofac;
using Bogus;
using FizzWare.NBuilder;
using FluentAssertions;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Tests.Services.Prelievi
{
    [TestFixture]
    public class Services_PrelieviTest
    {
        #region Constants

        private const int ID_COMUNE = 252;      // Filottrano (unico comune presente in test)
        private const int ID_PROFILO_TRASPORTATORE = 5;         // Trasportatore
        private const int ID_PROFILO_ALLEVATORE = 3;            // Allevatore
        private const int ID_TIPO_LATTE_QM = 1;                 // QM Alta Qualità

        private const int ID_ACQUIRENTE_DEFAULT = 123;
        private const int ID_DESTINATARIO_DEFAULT = 321;

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<PrelievoLatte, int> prelieviRepository;
        private IRepository<Trasbordo, long> trasbordiRepository;

        private IPrelieviLatteService prelieviLatteService;

        private DbCleaner dbCleaner;

        private long ID_TRASBORDO;

        #endregion

        #region Constructor

        public Services_PrelieviTest()
        {
            AutoFacConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();
            this.dbCleaner = new DbCleaner(uow);

            this.prelieviRepository = this.uow.Get<PrelievoLatte, int>();
            this.trasbordiRepository = this.uow.Get<Trasbordo, long>();

            this.prelieviLatteService = this.scope.Resolve<IPrelieviLatteService>();
        }

        #endregion

        #region Init / TearDown Methods

        [SetUp]
        public void Init()
        {
            // trasbordo
            var trabordiFaker = new Faker<Trasbordo>("it")
                .RuleFor(f => f.IMEI_Origine, f => f.Hacker.Abbreviation())
                .RuleFor(f => f.Targa_Origine, f => "EA-005-NE")
                .RuleFor(f => f.IMEI_Destinazione, f => f.Hacker.Abbreviation())
                .RuleFor(f => f.Targa_Destinazione, f => "FA-259-AD")
                .RuleFor(f => f.Lat, f => Convert.ToDecimal(f.Address.Latitude()))
                .RuleFor(f => f.Lng, f => Convert.ToDecimal(f.Address.Longitude()))
                .RuleFor(f => f.IdTemplateGiro, f => 1)
                .RuleFor(f => f.Data, f => DateTime.Now);

            var trasbordo = trabordiFaker.Generate();
            this.trasbordiRepository.Add(trasbordo);
            this.uow.SaveChanges();

            this.ID_TRASBORDO = trasbordo.Id;

            // prelievi
            var prelievi = new List<PrelievoLatte>();
            for (int i = 1; i <= 10; i++)
            {
                prelievi.Add(Builder<PrelievoLatte>
                    .CreateNew()
                        .With(p => p.IdTipoLatte = ID_TIPO_LATTE_QM)
                        .With(p => p.DataPrelievo = DateTime.Now)
                        .With(p => p.DataUltimaMungitura = DateTime.Now)
                        .With(p => p.IdTrasbordo = ID_TRASBORDO)
                    .Build()
                );
            }

            this.prelieviRepository.Add(prelievi);
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
        public void PrelieviLatteService_Sitra()
        {
            var prelieviDaInviare = this.prelieviLatteService.Sitra(DateTime.Today);

            // il caricamento dai prelievi sitra non deve troncare l'ora del prelievo e della mungitura
            prelieviDaInviare.All(p => p.DataPrelievo.Value.Hour != 0).Should().BeTrue();
            prelieviDaInviare.All(p => p.DataUltimaMungitura.Value.Hour != 0).Should().BeTrue();
        }

        [Test]
        public void PrelieviLatteService_Details()
        {
            var prelievo = this.prelieviRepository.Query.FirstOrDefault();
            var prelievoDto = this.prelieviLatteService.Details(prelievo.Id);

            prelievoDto.Trasbordo.Should().NotBeNull();
        }

        #endregion


    }
}
