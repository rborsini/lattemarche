using Autofac;
using FizzWare.NBuilder;
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
    public class Services_AziendaTrasportatoriTest
    {

        #region Constants

        private const int ID_COMUNE = 252;      // Filottrano (unico comune presente in test)

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<Utente, int> utentiRepository;
        private IRepository<AziendaTrasportatori, int> aziendeTrasportatoriRepository;
        private IRepository<TrasportatoreXAzienda, int> trasportatoreXAziendaRepository;

        private IUtentiService utentiService;

        private List<AziendaTrasportatori> aziende;
        private Utente utente;
        private TrasportatoreXAzienda trasportatoreXAzienda;

        #endregion

        #region Constructor

        public Services_AziendaTrasportatoriTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();


            this.utentiRepository = this.uow.Get<Utente, int>();
            this.aziendeTrasportatoriRepository = this.uow.Get<AziendaTrasportatori, int>();
            this.trasportatoreXAziendaRepository = this.uow.Get<TrasportatoreXAzienda, int>();

            this.utentiService = this.scope.Resolve<IUtentiService>();
        }

        #endregion

        #region Methods

        [SetUp]
        public void Init()
        {
            // aziende
            this.aziende = Builder<AziendaTrasportatori>
                .CreateListOfSize(5)
                .All()
                .Build()
                .ToList();

            this.aziendeTrasportatoriRepository.Add(aziende);
            this.uow.SaveChanges();

            // utente
            this.utente = Builder<Utente>
                .CreateNew()
                .Build();

            this.utentiRepository.Add(utente);
            this.uow.SaveChanges();

            // trasportatore x azienda
            this.trasportatoreXAzienda = new TrasportatoreXAzienda() { Id = this.utente.Id, IdAzienda = this.aziende[0].Id };

            this.trasportatoreXAziendaRepository.Add(trasportatoreXAzienda);
            this.uow.SaveChanges();
        }

        [TearDown]
        public void CleanUp()
        {
            var dbCleaner = new DbCleaner(this.uow);
            dbCleaner.CleanUp();
        }

        [Test]
        public void UtentiService_Create()
        {
            var idAzienda = this.aziende[0].Id;

            var utenteDto = Builder<UtenteDto>
                .CreateNew()
                    .With(u => u.IdComune = ID_COMUNE)
                    .With(u => u.IdAcquirente = (int?)null)
                    .With(u => u.IdCessionario = (int?)null)
                    .With(u => u.IdDestinatario = (int?)null)
                    .With(u => u.IdAziendaTrasporti = idAzienda)
                .Build();

            utenteDto = this.utentiService.Create(utenteDto);

            Assert.AreEqual(idAzienda, utenteDto.IdAziendaTrasporti.Value);
        }

        [Test]
        public void UtentiService_Details()
        {
            var utenteDto = this.utentiService.Details(this.utente.Id);

            Assert.IsTrue(utenteDto.IdAziendaTrasporti.HasValue);
            Assert.AreEqual(this.aziende[0].Id, utenteDto.IdAziendaTrasporti.Value);
        }


        [Test]
        public void UtentiService_Update()
        {
            var utenteDto = this.utentiService.Details(this.utente.Id);

            var idAzienda = this.aziende[1].Id;
            utenteDto.IdAziendaTrasporti = idAzienda;

            utenteDto = this.utentiService.Update(utenteDto);
            Assert.AreEqual(idAzienda, utenteDto.IdAziendaTrasporti.Value);

            utenteDto = Builder<UtenteDto>
                .CreateNew()
                    .With(u => u.IdAcquirente = null)
                    .With(u => u.IdDestinatario = null)
                    .With(u => u.IdCessionario = null)
                    .With(u => u.IdAziendaTrasporti = null)
                .Build();

            utenteDto = this.utentiService.Create(utenteDto);
            Assert.IsFalse(utenteDto.IdAziendaTrasporti.HasValue);

            utenteDto.IdAziendaTrasporti = idAzienda;
            utenteDto = this.utentiService.Update(utenteDto);
            Assert.AreEqual(idAzienda, utenteDto.IdAziendaTrasporti.Value);
        }

        [Test]
        public void UtentiService_Delete()
        {
            this.utentiService.Delete(this.utente.Id);

            var count = this.trasportatoreXAziendaRepository.DbSet.Count(uxa => uxa.Id == this.utente.Id);
            Assert.AreEqual(0, count);
        }

        #endregion

    }
}
