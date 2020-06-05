using Autofac;
using FizzWare.NBuilder;
using LatteMarche.Application.Autocisterne.Dtos;
using LatteMarche.Application.Autocisterne.Interfaces;
using LatteMarche.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Tests.Services.Autocisterne
{
    [TestFixture]
    [NonParallelizable]
    public class Services_AutocisterneTest
    {
        #region Constants

        private const int ID_COMUNE = 252;      // Filottrano (unico comune presente in test)

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<Autocisterna, int> autocisterneRepository;
        private IRepository<Utente, int> utentiRepository;
        private IAutocisterneService autocisterneService;

        private Utente trasportatore;

        #endregion

        #region Constructor

        public Services_AutocisterneTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();

            this.autocisterneRepository = this.uow.Get<Autocisterna, int>();
            this.utentiRepository = this.uow.Get<Utente, int>();
            this.autocisterneService = this.scope.Resolve<IAutocisterneService>();

        }

        #endregion

        #region Methods

        [SetUp]
        public void Init()
        {
            this.trasportatore = Builder<Utente>
                .CreateNew()
                    .With(u => u.IdComune = null)
                .Build();

            this.utentiRepository.Add(this.trasportatore);
            this.uow.SaveChanges();
        }

        [TearDown]
        public void CleanUp()
        {
            var dbCleaner = new DbCleaner(uow);
            dbCleaner.CleanUp();
        }

        [Test]
        public void AutocisterneTest_DropDown()
        {
            var cessionari = Builder<Autocisterna>
                .CreateListOfSize(5)
                    .All()
                        .With(a => a.IdTrasportatore = this.trasportatore.Id)
                .Build();

            this.autocisterneRepository.Add(cessionari);
            this.uow.SaveChanges();

            var dropdownDto = this.autocisterneService.DropDown(this.trasportatore.Id);
            Assert.AreEqual(5, dropdownDto.Items.Count);
        }

        [Test]
        public void AutocisterneTest_Details()
        {
            var autocisterna = Builder<Autocisterna>
                .CreateNew()
                    .With(a => a.IdTrasportatore = this.trasportatore.Id)
                .Build();

            autocisterna = this.autocisterneRepository.Add(autocisterna);
            this.uow.SaveChanges();

            var autocisternaDto = this.autocisterneService.Details(autocisterna.Id);
            Assert.IsNotNull(autocisternaDto);
        }

        [Test]
        public void AutocisterneTest_Create()
        {
            var autocisternaDto = Builder<AutocisternaDto>
                .CreateNew()
                    .With(a => a.IdTrasportatore = this.trasportatore.Id)
                .Build();

            autocisternaDto = this.autocisterneService.Create(autocisternaDto);
            Assert.IsNotNull(autocisternaDto);

            var cisterna = this.autocisterneRepository.GetById(autocisternaDto.Id);
            Assert.IsNotNull(cisterna);
        }

        [Test]
        public void AutocisterneTest_Update()
        {
            var autocisternaDto = Builder<AutocisternaDto>
                .CreateNew()
                    .With(a => a.IdTrasportatore = this.trasportatore.Id)
                .Build();

            autocisternaDto = this.autocisterneService.Create(autocisternaDto);

            autocisternaDto.Targa = "abc";
            autocisternaDto = this.autocisterneService.Update(autocisternaDto);
            Assert.AreEqual("abc", autocisternaDto.Targa);

            autocisternaDto = this.autocisterneService.Details(autocisternaDto.Id);
            Assert.AreEqual("abc", autocisternaDto.Targa);
        }

        [Test]
        public void AutocisterneTest_Delete()
        {
            var autocisterna = Builder<Autocisterna>
                .CreateNew()
                    .With(a => a.IdTrasportatore = this.trasportatore.Id)
                .Build();

            autocisterna = this.autocisterneRepository.Add(autocisterna);
            this.uow.SaveChanges();

            this.autocisterneService.Delete(autocisterna.Id);

            var autocisternaDto = this.autocisterneService.Details(autocisterna.Id);
            Assert.IsNull(autocisternaDto);

        }

        #endregion

    }
}

