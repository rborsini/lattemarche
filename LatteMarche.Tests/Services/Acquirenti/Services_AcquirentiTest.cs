using Autofac;
using FizzWare.NBuilder;
using LatteMarche.Application.Acquirenti.Dtos;
using LatteMarche.Application.Acquirenti.Interfaces;
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

namespace LatteMarche.Tests.Services.Acquirenti
{
    [TestFixture]
    [NonParallelizable]
    public class Services_AcquirentiTest
    {
        #region Constants

        private const int ID_COMUNE = 252;      // Filottrano (unico comune presente in test)


        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<Acquirente, int> acquirentiRepository;
        private IRepository<PrelievoLatte, int> prelieviRepository;

        private IAcquirentiService acquirentiService;
        private IUtentiService utentiService;

        private DbCleaner dbCleaner;

        private UtenteDto utenteAdmin;
        private UtenteDto utenteAllevatore;
        private UtenteDto utenteAcquirente;
        private UtenteDto utenteDestinatario;
        private UtenteDto utenteTrasportatore;

        #endregion

        #region Constructor

        public Services_AcquirentiTest()
        {
            AutoFacConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();

            this.acquirentiRepository = this.uow.Get<Acquirente, int>();
            this.acquirentiService = this.scope.Resolve<IAcquirentiService>();

            this.prelieviRepository = this.uow.Get<PrelievoLatte, int>();

            this.utentiService = this.scope.Resolve<IUtentiService>();

            this.dbCleaner = new DbCleaner(uow);
        }

        #endregion

        #region Methods

        [SetUp]
        public void Init()
        {
            var acquirenti = Builder<Acquirente>
                .CreateListOfSize(10)
                .TheFirst(5)
                    .With(a => a.Abilitato = true)
                .TheRest()
                    .With(a => a.Abilitato = false)
                .All()
                    .With(a => a.IdComune = ID_COMUNE)
                .Build();

            this.acquirentiRepository.Add(acquirenti);
            this.uow.SaveChanges();

            this.utenteAdmin = MakeUtente(ProfiloEnum.Admin);
            this.utenteAcquirente = MakeUtente(ProfiloEnum.Acquirente, acquirenti[0].Id);

            Random rnd = new Random();

            var prelievi = Builder<PrelievoLatte>
                .CreateListOfSize(100)
                .All()
                    .With(p => p.IdAcquirente = rnd.Next(0, 9))
                .Build();

            this.prelieviRepository.Add(prelievi);
            this.uow.SaveChanges();
        }

        private UtenteDto MakeUtente(ProfiloEnum profilo, int? idAcquirente = (int?)null, int? idCessionario = (int?)null, int? idDestinatario = (int?)null)
        {
            var utente = Builder<UtenteDto>
                .CreateNew()
                    .With(u => u.IdComune = null)
                    .With(u => u.IdProfilo = Convert.ToInt32(profilo))
                    .With(u => u.IdAcquirente = idAcquirente)
                    .With(u => u.IdCessionario = idCessionario)
                    .With(u => u.IdDestinatario = idDestinatario)
                .Build();

            return this.utentiService.Create(utente);
        }

        [TearDown]
        public void CleanUp()
        {
            this.dbCleaner.CleanUp();
        }

        [Test]
        public void AcquirentiTest_DropDown()
        {
            // admin vede tutto
            var dropdownDto = this.acquirentiService.DropDown(this.utenteAdmin.Id);
            Assert.AreEqual(5, dropdownDto.Items.Count);

            // acquirente vede solo  il  proprio
            dropdownDto = this.acquirentiService.DropDown(this.utenteAcquirente.Id);
            Assert.AreEqual(1, dropdownDto.Items.Count);
        }

        [Test]
        public void AcquirentiTest_Details()
        {
            var acquirente = Builder<Acquirente>
                .CreateNew()
                    .With(a => a.IdComune = ID_COMUNE)
                .Build();

            acquirente = this.acquirentiRepository.Add(acquirente);
            this.uow.SaveChanges();

            var acquirenteDto = this.acquirentiService.Details(acquirente.Id);
            Assert.IsNotNull(acquirenteDto);
        }

        [Test]
        public void AcquirentiTest_Create()
        {
            var acquirenteDto = Builder<AcquirenteDto>
                .CreateNew()
                    .With(a => a.IdComune = ID_COMUNE)
                .Build();

            acquirenteDto = this.acquirentiService.Create(acquirenteDto);
            Assert.IsNotNull(acquirenteDto);

            var acquirente = this.acquirentiRepository.GetById(acquirenteDto.Id);
            Assert.IsNotNull(acquirente);
            Assert.IsTrue(acquirente.Abilitato);
        }

        [Test]
        public void AcquirentiTest_Update()
        {
            var acquirenteDto = Builder<AcquirenteDto>
                .CreateNew()
                    .With(a => a.IdComune = ID_COMUNE)
                .Build();

            acquirenteDto = this.acquirentiService.Create(acquirenteDto);

            acquirenteDto.Indirizzo = "abc";
            acquirenteDto = this.acquirentiService.Update(acquirenteDto);
            Assert.AreEqual("abc", acquirenteDto.Indirizzo);

            acquirenteDto = this.acquirentiService.Details(acquirenteDto.Id);
            Assert.AreEqual("abc", acquirenteDto.Indirizzo);
        }

        [Test]
        public void AcquirentiTest_Delete()
        {
            var acquirente = Builder<Acquirente>
                .CreateNew()
                    .With(a => a.IdComune = ID_COMUNE)
                .Build();

            acquirente = this.acquirentiRepository.Add(acquirente);
            this.uow.SaveChanges();

            this.acquirentiService.Delete(acquirente.Id);

            var acquirenteDto = this.acquirentiService.Details(acquirente.Id);
            Assert.IsNull(acquirenteDto);

        }

        #endregion

    }
}
