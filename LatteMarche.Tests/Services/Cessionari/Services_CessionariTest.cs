using Autofac;
using FizzWare.NBuilder;
using LatteMarche.Application.Cessionari.Dtos;
using LatteMarche.Application.Cessionari.Interfaces;
using LatteMarche.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Tests.Services.Cessionari
{
    [TestFixture]
    [NonParallelizable]
    public class Services_CessionariTest
    {
        #region Constants

        private const int ID_COMUNE = 252;      // Filottrano (unico comune presente in test)

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<Cessionario, int> cessionariRepository;
        private ICessionariService cessionariService;

        private DbCleaner dbCleaner;

        #endregion

        #region Constructor

        public Services_CessionariTest()
        {
            AutoFacConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();

            this.cessionariRepository = this.uow.Get<Cessionario, int>();
            this.cessionariService = this.scope.Resolve<ICessionariService>();

            this.dbCleaner = new DbCleaner(uow);
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
        public void CessionariTest_DropDown()
        {
            var cessionari = Builder<Cessionario>
                .CreateListOfSize(5)
                    .All()
                        .With(a => a.IdComune = ID_COMUNE)
                .Build();

            this.cessionariRepository.Add(cessionari);
            this.uow.SaveChanges();

            var dropdownDto = this.cessionariService.DropDown();
            Assert.AreEqual(5, dropdownDto.Items.Count);
        }

        [Test]
        public void CessionariTest_Details()
        {
            var cessionario = Builder<Cessionario>
                .CreateNew()
                    .With(a => a.IdComune = ID_COMUNE)
                .Build();

            cessionario = this.cessionariRepository.Add(cessionario);
            this.uow.SaveChanges();

            var cessionarioDto = this.cessionariService.Details(cessionario.Id);
            Assert.IsNotNull(cessionarioDto);
        }

        [Test]
        public void CessionariTest_Create()
        {
            var cessionarioDto = Builder<CessionarioDto>
                .CreateNew()
                    .With(a => a.IdComune = ID_COMUNE)
                .Build();

            cessionarioDto = this.cessionariService.Create(cessionarioDto);
            Assert.IsNotNull(cessionarioDto);

            var cessionario = this.cessionariRepository.GetById(cessionarioDto.Id);
            Assert.IsNotNull(cessionario);
            Assert.IsTrue(cessionario.Abilitato);
        }

        [Test]
        public void CessionariTest_Update()
        {
            var cessionarioDto = Builder<CessionarioDto>
                .CreateNew()
                    .With(a => a.IdComune = ID_COMUNE)
                .Build();

            cessionarioDto = this.cessionariService.Create(cessionarioDto);

            cessionarioDto.Indirizzo = "abc";
            cessionarioDto = this.cessionariService.Update(cessionarioDto);
            Assert.AreEqual("abc", cessionarioDto.Indirizzo);

            cessionarioDto = this.cessionariService.Details(cessionarioDto.Id);
            Assert.AreEqual("abc", cessionarioDto.Indirizzo);
        }

        [Test]
        public void CessionariTest_Delete()
        {
            var cessionario = Builder<Cessionario>
                .CreateNew()
                    .With(a => a.IdComune = ID_COMUNE)
                .Build();

            cessionario = this.cessionariRepository.Add(cessionario);
            this.uow.SaveChanges();

            this.cessionariService.Delete(cessionario.Id);

            var cessionarioDto = this.cessionariService.Details(cessionario.Id);
            Assert.IsNull(cessionarioDto);

        }

        #endregion

    }
}
