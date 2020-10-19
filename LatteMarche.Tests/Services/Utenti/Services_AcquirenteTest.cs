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
    public class Services_AcquirenteTest
    {

        #region Constants

        private const int ID_COMUNE = 252;      // Filottrano (unico comune presente in test)

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<Utente, int> utentiRepository;
        private IRepository<Acquirente, int> acquirentiRepository;
        private IRepository<UtenteXAcquirente, int> utenteXAcquirenteRepository;

        private IUtentiService utentiService;

        private List<Acquirente> acquirenti;
        private Utente utente;
        private UtenteXAcquirente utenteXAcquirente;

        #endregion

        #region Constructor

        public Services_AcquirenteTest()
        {
            AutoFacConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();
            

            this.utentiRepository = this.uow.Get<Utente, int>();
            this.acquirentiRepository = this.uow.Get<Acquirente, int>();
            this.utenteXAcquirenteRepository = this.uow.Get<UtenteXAcquirente, int>();

            this.utentiService = this.scope.Resolve<IUtentiService>();
        }

        #endregion

        #region Methods

        [SetUp]
        public void Init()
        {
            // acquirenti
            this.acquirenti = Builder<Acquirente>
                .CreateListOfSize(5)
                .All()
                    .With(a => a.IdComune = ID_COMUNE)
                .Build()
                .ToList();

            this.acquirentiRepository.Add(acquirenti);
            this.uow.SaveChanges();

            // utente
            this.utente = Builder<Utente>
                .CreateNew()
                    .With(a => a.IdComune = ID_COMUNE)
                .Build();

            this.utentiRepository.Add(utente);
            this.uow.SaveChanges();

            // utente x acquirente
            this.utenteXAcquirente = new UtenteXAcquirente() { Id = this.utente.Id, IdAcquirente = this.acquirenti[0].Id };

            this.utenteXAcquirenteRepository.Add(utenteXAcquirente);
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
            var idAcquirente = this.acquirenti[0].Id;

            var utenteDto = Builder<UtenteDto>
                .CreateNew()
                    .With(u => u.IdComune = ID_COMUNE)
                    .With(u => u.IdAcquirente = idAcquirente)
                    .With(u => u.IdCessionario = (int?)null)
                    .With(u => u.IdDestinatario = (int?)null)
                    .With(u => u.IdAziendaTrasporti = (int?)null)
                .Build();

            utenteDto = this.utentiService.Create(utenteDto);

            Assert.AreEqual(idAcquirente, utenteDto.IdAcquirente.Value);
        }

        [Test]
        public void UtentiService_Details()
        {
            var utenteDto = this.utentiService.Details(this.utente.Id);

            Assert.IsTrue(utenteDto.IdAcquirente.HasValue);
            Assert.AreEqual(this.acquirenti[0].Id, utenteDto.IdAcquirente.Value);
        }


        [Test]
        public void UtentiService_Update()
        {
            var utenteDto = this.utentiService.Details(this.utente.Id);

            var idAcquirente = this.acquirenti[1].Id;
            utenteDto.IdAcquirente = idAcquirente;

            utenteDto = this.utentiService.Update(utenteDto);
            Assert.AreEqual(idAcquirente, utenteDto.IdAcquirente.Value);

            utenteDto = Builder<UtenteDto>
                .CreateNew()
                    .With(u => u.IdAcquirente = null)
                    .With(u => u.IdDestinatario = null)
                    .With(u => u.IdCessionario = null)
                    .With(u => u.IdAziendaTrasporti = (int?)null)
                .Build();

            utenteDto = this.utentiService.Create(utenteDto);
            Assert.IsFalse(utenteDto.IdAcquirente.HasValue);

            utenteDto.IdAcquirente = idAcquirente;
            utenteDto = this.utentiService.Update(utenteDto);
            Assert.AreEqual(idAcquirente, utenteDto.IdAcquirente.Value);
        }

        [Test]
        public void UtentiService_Delete()
        {
            this.utentiService.Delete(this.utente.Id);

            var count = this.utenteXAcquirenteRepository.DbSet.Count(uxa => uxa.Id == this.utente.Id);
            Assert.AreEqual(0, count);
        }

        #endregion

    }
}
