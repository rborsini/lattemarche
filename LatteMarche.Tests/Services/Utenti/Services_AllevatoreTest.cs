using Autofac;
using FizzWare.NBuilder;
using LatteMarche.Application.Allevamenti.Dtos;
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
    public class Services_AllevatoreTest 
    {

        #region Constants

        private const int ID_COMUNE = 252;      // Filottrano (unico comune presente in test)

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<Utente, int> utentiRepository;
        private IRepository<Allevamento, int> allevamentiRepository;

        private IUtentiService utentiService;

        private Utente utente;
        private Allevamento allevamento;

        #endregion

        #region Constructor

        public Services_AllevatoreTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();

            this.utentiRepository = this.uow.Get<Utente, int>();
            this.allevamentiRepository = this.uow.Get<Allevamento, int>();

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

            // allevamento
            this.allevamento = Builder<Allevamento>
                .CreateNew()
                    .With(a => a.IdUtente = this.utente.Id)
                    .With(u => u.CodiceAsl = "abcd")
                .Build();

            this.allevamentiRepository.Add(allevamento);
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
            var allevamentoDto = Builder<AllevamentoDto>
                .CreateNew()
                    .With(u => u.IdComune = ID_COMUNE)
                .Build();

            var utenteDto = Builder<UtenteDto>
                .CreateNew()
                    .With(u => u.IdComune = ID_COMUNE)
                    .With(u => u.IdAcquirente = (int?)null)
                    .With(u => u.IdCessionario = (int?)null)
                    .With(u => u.IdDestinatario = (int?)null)
                    .With(u => u.IdAziendaTrasporti = (int?)null)
                    .With(u => u.Allevamenti = new List<AllevamentoDto>() { allevamentoDto })
                .Build();

            utenteDto = this.utentiService.Create(utenteDto);

            Assert.AreEqual(1, utenteDto.Allevamenti.Count);
        }

        [Test]
        public void UtentiService_Details()
        {
            var utenteDto = this.utentiService.Details(this.utente.Id);
            Assert.AreEqual(1, utenteDto.Allevamenti.Count);
        }

        [Test]
        public void UtentiService_Update()
        {
            // aggiunta nuovo allevamento
            var utenteDto = this.utentiService.Details(this.utente.Id);

            var allevamentoDto = Builder<AllevamentoDto>
                .CreateNew()
                    .With(u => u.Id = 0)
                    .With(u => u.IdUtente = this.utente.Id)
                    .With(u => u.IdComune = ID_COMUNE)
                .Build();

            utenteDto.Allevamenti.Add(allevamentoDto);
            utenteDto = this.utentiService.Update(utenteDto);
            Assert.AreEqual(2, utenteDto.Allevamenti.Count);

            // editazione allevamento 
            utenteDto.Allevamenti[0].CUAA = "1234";

            utenteDto = this.utentiService.Update(utenteDto);
            Assert.AreEqual("1234", utenteDto.Allevamenti[0].CUAA);
            Assert.That(utenteDto.Allevamenti[0].IdUtente.HasValue, Is.True);
            Assert.That(utenteDto.Allevamenti[0].IdUtente.Value, Is.EqualTo(this.utente.Id));

            // #Bug 326114
            var allevamenti = this.allevamentiRepository.DbSet.Where(a => a.CodiceAsl == "abcd").ToList();
            Assert.That(allevamenti, Has.Count.EqualTo(1));

            // rimozione allevamento
            utenteDto.Allevamenti.RemoveAt(0);

            utenteDto = this.utentiService.Update(utenteDto);
            Assert.AreEqual(1, utenteDto.Allevamenti.Count);

        }

        [Test]
        public void UtentiService_Delete()
        {
            this.utentiService.Delete(this.utente.Id);

            var count = this.allevamentiRepository.DbSet.Count(uxa => uxa.IdUtente == this.utente.Id);
            Assert.AreEqual(0, count);
        }

        #endregion

    }
}
