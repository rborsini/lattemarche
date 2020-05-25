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
    public class Services_DestinatarioTest
    {

        #region Constants

        private const int ID_COMUNE = 252;      // Filottrano (unico comune presente in test)

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<Utente, int> utentiRepository;
        private IRepository<Destinatario, int> destinatariRepository;
        private IRepository<UtenteXDestinatario, int> utenteXDestinatarioRepository;

        private IUtentiService utentiService;

        private List<Destinatario> destinatari;
        private Utente utente;
        private UtenteXDestinatario utenteXDestinatario;

        #endregion

        #region Constructor

        public Services_DestinatarioTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();


            this.utentiRepository = this.uow.Get<Utente, int>();
            this.destinatariRepository = this.uow.Get<Destinatario, int>();
            this.utenteXDestinatarioRepository = this.uow.Get<UtenteXDestinatario, int>();

            this.utentiService = this.scope.Resolve<IUtentiService>();
        }

        #endregion

        #region Methods

        [SetUp]
        public void Init()
        {
            // destinatario
            this.destinatari = Builder<Destinatario>
                .CreateListOfSize(5)
                .All()
                    .With(a => a.IdComune = ID_COMUNE)
                .Build()
                .ToList();

            this.destinatariRepository.Add(destinatari);
            this.uow.SaveChanges();

            // utente
            this.utente = Builder<Utente>
                .CreateNew()
                    .With(a => a.IdComune = ID_COMUNE)
                .Build();

            this.utentiRepository.Add(utente);
            this.uow.SaveChanges();

            // utente x destinatario
            this.utenteXDestinatario = new UtenteXDestinatario() { Id = this.utente.Id, IdDestinatario = this.destinatari[0].Id };

            this.utenteXDestinatarioRepository.Add(utenteXDestinatario);
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
            var idDestinatario = this.destinatari[0].Id;

            var utenteDto = Builder<UtenteDto>
                .CreateNew()
                    .With(u => u.IdComune = ID_COMUNE)
                    .With(u => u.IdAcquirente = (int?)null)
                    .With(u => u.IdCessionario = (int?)null)
                    .With(u => u.IdDestinatario = idDestinatario)
                    .With(u => u.IdAziendaTrasporti = (int?)null)
                .Build();

            utenteDto = this.utentiService.Create(utenteDto);

            Assert.AreEqual(idDestinatario, utenteDto.IdDestinatario.Value);
        }

        [Test]
        public void UtentiService_Details()
        {
            var utenteDto = this.utentiService.Details(this.utente.Id);

            Assert.IsTrue(utenteDto.IdDestinatario.HasValue);
            Assert.AreEqual(this.destinatari[0].Id, utenteDto.IdDestinatario.Value);
        }


        [Test]
        public void UtentiService_Update()
        {
            var utenteDto = this.utentiService.Details(this.utente.Id);

            var idDestinatario = this.destinatari[1].Id;
            utenteDto.IdDestinatario = idDestinatario;

            utenteDto = this.utentiService.Update(utenteDto);
            Assert.AreEqual(idDestinatario, utenteDto.IdDestinatario.Value);

            utenteDto = Builder<UtenteDto>
                .CreateNew()
                    .With(u => u.IdAcquirente = null)
                    .With(u => u.IdCessionario = null)
                    .With(u => u.IdDestinatario = null)
                    .With(u => u.IdAziendaTrasporti = (int?)null)
                .Build();

            utenteDto = this.utentiService.Create(utenteDto);
            Assert.IsFalse(utenteDto.IdDestinatario.HasValue);

            utenteDto.IdDestinatario = idDestinatario;
            utenteDto = this.utentiService.Update(utenteDto);
            Assert.AreEqual(idDestinatario, utenteDto.IdDestinatario.Value);
        }

        [Test]
        public void UtentiService_Delete()
        {
            this.utentiService.Delete(this.utente.Id);

            var count = this.utenteXDestinatarioRepository.DbSet.Count(uxa => uxa.Id == this.utente.Id);
            Assert.AreEqual(0, count);
        }

        #endregion

    }
}
