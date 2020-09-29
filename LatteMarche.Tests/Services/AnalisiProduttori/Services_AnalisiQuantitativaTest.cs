using Autofac;
using FizzWare.NBuilder;
using LatteMarche.Application.Dashboard.Interfaces;
using LatteMarche.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Tests.Services.AnalisiProduttori
{
    [TestFixture]
    [NonParallelizable]
    public class Services_AnalisiQuantitativaTest
    {
        #region Constants

        private const int ID_ALLEVAMENTO = 1;

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<PrelievoLatte, int> prelieviRepository;

        private IAnalisiQuantitativaService service;

        private DbCleaner dbCleaner;

        #endregion

        #region Constructor

        public Services_AnalisiQuantitativaTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();

            this.prelieviRepository = this.uow.Get<PrelievoLatte, int>();

            this.service = this.scope.Resolve<IAnalisiQuantitativaService>();

            this.dbCleaner = new DbCleaner(uow);
        }

        #endregion

        #region Methods

        [SetUp]
        public void Init()
        {
            var prelievi = new List<PrelievoLatte>();
            var date = new DateTime(DateTime.Now.Year, 1, 1);
            var end = date.AddYears(1);

            while(date < end)
            {
                var prelievo = Builder<PrelievoLatte>
                    .CreateNew()
                        .With(p => p.DataPrelievo = date)
                        .With(p => p.IdAllevamento = ID_ALLEVAMENTO)
                    .Build();

                prelievi.Add(prelievo);

                date = date.AddDays(1);
            }

            this.prelieviRepository.Add(prelievi);
            this.uow.SaveChanges();
        }

        [TearDown]
        public void CleanUp()
        {
            this.dbCleaner.CleanUp();
        }

        [Test]
        public void AnalisiQuantitativaTest_Load()
        {
            var idAllevamento = ID_ALLEVAMENTO;
            var from = DateTime.Today.AddMonths(-1);
            var to = DateTime.Today;

            var numGiorni = (to - from).TotalDays;

            var dto = this.service.Load(idAllevamento, from, to);
            Assert.IsNotNull(dto);

            Assert.AreEqual(numGiorni, dto.Records.Count);
            Assert.AreEqual(numGiorni, dto.AndamentoGiornaliero.ValoriAsseX.Count);
            Assert.AreEqual(12, dto.AndamentoMensile.ValoriAsseX.Count);
        }


        #endregion
    }
}
