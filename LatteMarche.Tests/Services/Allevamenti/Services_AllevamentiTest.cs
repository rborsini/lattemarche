using Autofac;
using FizzWare.NBuilder;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Tests.Services.Allevamenti
{
    [TestFixture]
    public class Services_AllevamentiTest
    {
        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<Allevamento, int> allevamentiRepository;

        private IAllevamentiService allevamentiService;

        private DbCleaner dbCleaner;

        #endregion

        #region Constructor

        public Services_AllevamentiTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = scope.Resolve<IUnitOfWork>();
            this.dbCleaner = new DbCleaner(uow);

            this.allevamentiRepository = this.uow.Get<Allevamento, int>();

            this.allevamentiService = this.scope.Resolve<IAllevamentiService>();

        }

        #endregion

        #region Methods

        [SetUp]
        public void Init()
        {

        }

        [TearDown]
        public void CleanUp()
        {
            this.dbCleaner.CleanUp();
        }

        [Test]
        public void AllevamentiService_Search()
        {
            int size = 10;

            // 10 allevamenti
            var allevamenti = Builder<Allevamento>
                .CreateListOfSize(size)
                .All()
                .Build();

            this.allevamentiRepository.Add(allevamenti);
            this.uow.SaveChanges();

            // ricerca senza parametri 
            var list = this.allevamentiService.Search(null);
            Assert.IsNotNull(list);
            Assert.AreEqual(size, list.Count);
        }

        [Test]
        public void AllevamentiService_GetAllevamentiSitra()
        {
            int size = 10;
            int allevamentiNonSitra = 5;

            // 10 allevamenti
            var allevamenti = Builder<Allevamento>
                .CreateListOfSize(size)
                .All()
                    .TheFirst(allevamentiNonSitra)
                        .With(a => a.CUAA = "")
                .Build();

            this.allevamentiRepository.Add(allevamenti);
            this.uow.SaveChanges();

            // ricerca allevamenti sitra
            var list = this.allevamentiService.GetAllevamentiSitra();
            Assert.AreEqual(size - allevamentiNonSitra, list.Count);
        }

        [Test]
        public void AllevamentiService_Create()
        {
            var allevamentoDto = Builder<AllevamentoDto>
                .CreateNew()
                .Build();

            allevamentoDto = this.allevamentiService.Create(allevamentoDto);

            Assert.IsNotNull(allevamentoDto);
        }

        [Test]
        public void AllevamentiService_Update()
        {
            var allevamento = Builder<Allevamento>
                .CreateNew()
                .Build();

            this.allevamentiRepository.Add(allevamento);
            this.uow.SaveChanges();

            var allevamentoDto = this.allevamentiService.Details(allevamento.Id);

            allevamentoDto.CUAA = "ABC";

            allevamentoDto = this.allevamentiService.Update(allevamentoDto);
            Assert.AreEqual("ABC", allevamentoDto.CUAA);
        }

        [Test]
        public void AllevamentiService_Details()
        {
            var allevamento = Builder<Allevamento>
                .CreateNew()
                .Build();

            this.allevamentiRepository.Add(allevamento);
            this.uow.SaveChanges();

            var allevamentoDto = this.allevamentiService.Details(allevamento.Id);
            Assert.AreEqual(allevamentoDto.CUAA, allevamento.CUAA);
        }

        #endregion

    }
}
