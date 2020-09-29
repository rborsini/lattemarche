using Autofac;
using FizzWare.NBuilder;
using LatteMarche.Application.AnalisiLatte.Dtos;
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
    public class Services_AnalisiComparativaTest
    {
        #region Constants

        private const int ID_ALLEVAMENTO = 1;

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<Analisi, string> analisiRepository;

        private IAnalisiComparativaService service;

        private DbCleaner dbCleaner;

        #endregion

        #region Constructor

        public Services_AnalisiComparativaTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();

            this.analisiRepository = this.uow.Get<Analisi, string>();

            this.service = this.scope.Resolve<IAnalisiComparativaService>();

            this.dbCleaner = new DbCleaner(uow);
        }

        #endregion

        #region Methods

        [SetUp]
        public void Init()
        {
            var analisiList = new List<Analisi>();
            var rnd = new Random();

            for (var i = 1; i <= 3; i++)
            {
                var date = new DateTime(DateTime.Now.Year, 1, 1);
                var end = date.AddYears(1);

                while (date < end)
                {
                    var analisi = Builder<Analisi>
                        .CreateNew()
                            .With(p => p.Id = Guid.NewGuid().ToString())
                            .With(p => p.DataPrelievo = date)
                            .With(p => p.IdAllevamento = i)
                            .With(p => p.Valori = new List<ValoreAnalisi>()
                            {
                            Builder<ValoreAnalisi>.CreateNew().With(v => v.Valore = $"{rnd.Next(100)}").With(v => v.Nome = AnalisiDto.GRASSO).Build(),
                            Builder<ValoreAnalisi>.CreateNew().With(v => v.Valore = $"{rnd.Next(100)}").With(v => v.Nome = AnalisiDto.PROTEINE).Build(),
                            Builder<ValoreAnalisi>.CreateNew().With(v => v.Valore = $"{rnd.Next(100)}").With(v => v.Nome = AnalisiDto.CARICA_BATTERICA).Build(),
                            Builder<ValoreAnalisi>.CreateNew().With(v => v.Valore = $"{rnd.Next(100)}").With(v => v.Nome = AnalisiDto.CELLULE_SOMATICHE).Build(),
                            })
                        .Build();

                    analisiList.Add(analisi);

                    date = date.AddDays(1);
                }

                this.analisiRepository.Add(analisiList);
                this.uow.SaveChanges();
                analisiList.Clear();
            }


        }

        [TearDown]
        public void CleanUp()
        {
            this.dbCleaner.CleanUp();
        }

        [Test]
        public void AnalisiQualitativaTest_Load()
        {
            var idAllevamento = ID_ALLEVAMENTO;
            var from = DateTime.Today.AddMonths(-1);
            var to = DateTime.Today;

            var numGiorni = (to - from).TotalDays;

            var dto = this.service.Load(idAllevamento, from, to);
            Assert.IsNotNull(dto);
        }


        #endregion
    }
}
