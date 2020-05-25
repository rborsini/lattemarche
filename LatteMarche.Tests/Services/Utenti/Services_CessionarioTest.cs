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
    public class Services_CessionarioTest
    {

        #region Constants

        private const int ID_COMUNE = 252;      // Filottrano (unico comune presente in test)

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<Utente, int> utentiRepository;
        private IRepository<Cessionario, int> cessionariRepository;
        private IRepository<UtenteXCessionario, int> utenteXCessionarioRepository;

        private IUtentiService utentiService;

        private List<Cessionario> cessionari;
        private Utente utente;
        private UtenteXCessionario utenteXCessionario;

        #endregion

        #region Constructor

        public Services_CessionarioTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();


            this.utentiRepository = this.uow.Get<Utente, int>();
            this.cessionariRepository = this.uow.Get<Cessionario, int>();
            this.utenteXCessionarioRepository = this.uow.Get<UtenteXCessionario, int>();

            this.utentiService = this.scope.Resolve<IUtentiService>();
        }

        #endregion

        #region Methods

        [SetUp]
        public void Init()
        {
            // cessionari
            this.cessionari = Builder<Cessionario>
                .CreateListOfSize(5)
                .All()
                    .With(a => a.IdComune = ID_COMUNE)
                .Build()
                .ToList();

            this.cessionariRepository.Add(cessionari);
            this.uow.SaveChanges();

            // utente
            this.utente = Builder<Utente>
                .CreateNew()
                    .With(a => a.IdComune = ID_COMUNE)
                .Build();

            this.utentiRepository.Add(utente);
            this.uow.SaveChanges();

            // utente x cessionario
            this.utenteXCessionario = new UtenteXCessionario() { Id = this.utente.Id, IdCessionario = this.cessionari[0].Id };

            this.utenteXCessionarioRepository.Add(utenteXCessionario);
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
            var idCessionario = this.cessionari[0].Id;

            var utenteDto = Builder<UtenteDto>
                .CreateNew()
                    .With(u => u.IdComune = ID_COMUNE)
                    .With(u => u.IdAcquirente = (int?)null)
                    .With(u => u.IdCessionario = idCessionario)
                    .With(u => u.IdDestinatario = (int?)null)
                    .With(u => u.IdAziendaTrasporti = (int?)null)
                .Build();

            utenteDto = this.utentiService.Create(utenteDto);

            Assert.AreEqual(idCessionario, utenteDto.IdCessionario.Value);
        }

        [Test]
        public void UtentiService_Details()
        {
            var utenteDto = this.utentiService.Details(this.utente.Id);

            Assert.IsTrue(utenteDto.IdCessionario.HasValue);
            Assert.AreEqual(this.cessionari[0].Id, utenteDto.IdCessionario.Value);
        }


        [Test]
        public void UtentiService_Update()
        {
            var utenteDto = this.utentiService.Details(this.utente.Id);

            var idCessionario = this.cessionari[1].Id;
            utenteDto.IdCessionario = idCessionario;

            utenteDto = this.utentiService.Update(utenteDto);
            Assert.AreEqual(idCessionario, utenteDto.IdCessionario.Value);

            utenteDto = Builder<UtenteDto>
                .CreateNew()
                    .With(u => u.IdAcquirente = null)
                    .With(u => u.IdCessionario = null)
                    .With(u => u.IdDestinatario = null)
                    .With(u => u.IdAziendaTrasporti = (int?)null)
                .Build();

            utenteDto = this.utentiService.Create(utenteDto);
            Assert.IsFalse(utenteDto.IdCessionario.HasValue);

            utenteDto.IdCessionario = idCessionario;
            utenteDto = this.utentiService.Update(utenteDto);
            Assert.AreEqual(idCessionario, utenteDto.IdCessionario.Value);
        }

        [Test]
        public void UtentiService_Delete()
        {
            this.utentiService.Delete(this.utente.Id);

            var count = this.utenteXCessionarioRepository.DbSet.Count(uxa => uxa.Id == this.utente.Id);
            Assert.AreEqual(0, count);
        }

        #endregion

    }
}

