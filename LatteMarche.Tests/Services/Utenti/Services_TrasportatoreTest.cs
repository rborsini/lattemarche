using Autofac;
using FizzWare.NBuilder;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Autocisterne.Dtos;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Tests.Services.Utenti
{
    [TestFixture]
    [NonParallelizable]
    public class Services_TrasportatoreTest
    {


        #region Constants

        private const int ID_COMUNE = 252;      // Filottrano (unico comune presente in test)

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<Utente, int> utentiRepository;
        private IRepository<Autocisterna, int> autocisterneRepository;

        private IUtentiService utentiService;

        private Utente utente;
        private Autocisterna autocisterna;

        #endregion

        #region Constructor

        public Services_TrasportatoreTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();

            this.utentiRepository = this.uow.Get<Utente, int>();
            this.autocisterneRepository = this.uow.Get<Autocisterna, int>();

            this.utentiService = this.scope.Resolve<IUtentiService>();
        }

        #endregion

        #region Methods

        [SetUp]
        public void Init()
        {
            // utente
            this.utente = Builder<Utente>
                .CreateNew()
                    .With(a => a.IdComune = ID_COMUNE)
                .Build();

            this.utentiRepository.Add(utente);
            this.uow.SaveChanges();

            // autocisterna
            this.autocisterna = Builder<Autocisterna>
                .CreateNew()
                    .With(a => a.IdTrasportatore = this.utente.Id)
                .Build();

            this.autocisterneRepository.Add(autocisterna);
            this.uow.SaveChanges();
        }


        [TearDown]
        public void CleanUp()
        {
            var dbCleaner = new DbCleaner(uow);
            dbCleaner.CleanUp();
        }

        [Test]
        public void UtentiService_Create()
        {
            var autocisternaDto = Builder<AutocisternaDto>
                .CreateNew()
                .Build();

            var utenteDto = Builder<UtenteDto>
                .CreateNew()
                    .With(u => u.IdComune = ID_COMUNE)
                    .With(u => u.IdAcquirente = (int?)null)
                    .With(u => u.IdCessionario = (int?)null)
                    .With(u => u.IdDestinatario = (int?)null)
                    .With(u => u.IdAziendaTrasporti = (int?)null)
                    .With(u => u.Autocisterne = new List<AutocisternaDto>() { autocisternaDto })
                .Build();

            utenteDto = this.utentiService.Create(utenteDto);

            Assert.AreEqual(1, utenteDto.Autocisterne.Count);
        }

        [Test]
        public void UtentiService_Details()
        {
            var utenteDto = this.utentiService.Details(this.utente.Id);
            Assert.AreEqual(1, utenteDto.Autocisterne.Count);
        }

        [Test]
        public void UtentiService_Update()
        {
            // aggiunta nuova autocisterna
            var utenteDto = this.utentiService.Details(this.utente.Id);

            var autocisternaDto = Builder<AutocisternaDto>
                .CreateNew()
                    .With(u => u.Id = 0)
                    .With(u => u.IdTrasportatore = this.utente.Id)
                .Build();

            utenteDto.Autocisterne.Add(autocisternaDto);
            utenteDto = this.utentiService.Update(utenteDto);
            Assert.AreEqual(2, utenteDto.Autocisterne.Count);

            // editazione autocisterna 
            utenteDto.Autocisterne[0].Targa = "1234";

            utenteDto = this.utentiService.Update(utenteDto);
            Assert.AreEqual("1234", utenteDto.Autocisterne[0].Targa);

            // rimozione autocisterna
            utenteDto.Autocisterne.RemoveAt(0);

            utenteDto = this.utentiService.Update(utenteDto);
            Assert.AreEqual(1, utenteDto.Autocisterne.Count);

        }

        [Test]
        public void UtentiService_Delete()
        {
            this.utentiService.Delete(this.utente.Id);

            var count = this.autocisterneRepository.DbSet.Count(uxa => uxa.IdTrasportatore == this.utente.Id);
            Assert.AreEqual(0, count);
        }

        #endregion

    }
}
