using Autofac;
using FizzWare.NBuilder;
using LatteMarche.Application.AziendeTrasportatori.Dtos;
using LatteMarche.Application.AziendeTrasportatori.Interfaces;
using LatteMarche.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Tests.Services.AziendeTrasportatori
{
    [TestFixture]
    [NonParallelizable]
    public class Services_AziendeTrasportatoriTest
    {
        #region Constants

        private const int ID_COMUNE = 252;      // Filottrano (unico comune presente in test)

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<AziendaTrasportatori, int> aziendeTrasportatoriRepository;
        private IAziendeTrasportatoriService aziendeTrasportatoriService;

        private DbCleaner dbCleaner;

        #endregion

        #region Constructor

        public Services_AziendeTrasportatoriTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();

            this.aziendeTrasportatoriRepository = this.uow.Get<AziendaTrasportatori, int>();
            this.aziendeTrasportatoriService = this.scope.Resolve<IAziendeTrasportatoriService>();

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
        public void AziendeTrasportatoriTest_DropDown()
        {
            var aziende = Builder<AziendaTrasportatori>
                .CreateListOfSize(5)
                .Build();

            this.aziendeTrasportatoriRepository.Add(aziende);
            this.uow.SaveChanges();

            var dropdownDto = this.aziendeTrasportatoriService.DropDown();
            Assert.AreEqual(5, dropdownDto.Items.Count);
        }

        [Test]
        public void AziendeTrasportatoriTest_Details()
        {
            var azienda = Builder<AziendaTrasportatori>
                .CreateNew()
                .Build();

            azienda = this.aziendeTrasportatoriRepository.Add(azienda);
            this.uow.SaveChanges();

            var aziendaDto = this.aziendeTrasportatoriService.Details(azienda.Id);
            Assert.IsNotNull(aziendaDto);
        }

        [Test]
        public void AziendeTrasportatoriTest_Create()
        {
            var aziendaDto = Builder<AziendaTrasportatoriDto>
                .CreateNew()
                .Build();

            aziendaDto = this.aziendeTrasportatoriService.Create(aziendaDto);
            Assert.IsNotNull(aziendaDto);

            var azienda = this.aziendeTrasportatoriRepository.GetById(aziendaDto.Id);
            Assert.IsNotNull(azienda);
        }

        [Test]
        public void AziendeTrasportatoriTest_Update()
        {
            var aziendaDto = Builder<AziendaTrasportatoriDto>
                .CreateNew()
                .Build();

            aziendaDto = this.aziendeTrasportatoriService.Create(aziendaDto);

            aziendaDto.RagioneSociale = "abc";
            aziendaDto = this.aziendeTrasportatoriService.Update(aziendaDto);
            Assert.AreEqual("abc", aziendaDto.RagioneSociale);

            aziendaDto = this.aziendeTrasportatoriService.Details(aziendaDto.Id);
            Assert.AreEqual("abc", aziendaDto.RagioneSociale);
        }

        [Test]
        public void AziendeTrasportatoriTest_Delete()
        {
            var azienda = Builder<AziendaTrasportatori>
                .CreateNew()
                .Build();

            azienda = this.aziendeTrasportatoriRepository.Add(azienda);
            this.uow.SaveChanges();

            this.aziendeTrasportatoriService.Delete(azienda.Id);

            var aziendaDto = this.aziendeTrasportatoriService.Details(azienda.Id);
            Assert.IsNull(aziendaDto);

        }

        #endregion

    }
}
