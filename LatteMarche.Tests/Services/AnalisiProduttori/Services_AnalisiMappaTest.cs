using Autofac;
using FizzWare.NBuilder;
using LatteMarche.Application.Dashboard.Filters;
using LatteMarche.Application.Dashboard.Interfaces;
using LatteMarche.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Tests.Services.AnalisiProduttori
{
    [TestFixture]
    [NonParallelizable]
    public class Services_AnalisiMappaTest
    {
        #region Constants

        private const int TIPO_LATTE_PECORA = 6;

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<Acquirente, int> acquirentiRepository;
        private IRepository<Allevamento, int> allevamentiRepository;
        private IRepository<Utente, int> utentiRepository;

        private IAnalisiMappaService service;

        private DbCleaner dbCleaner;

        #endregion

        #region Constructor

        public Services_AnalisiMappaTest()
        {
            AutoFacConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();

            this.acquirentiRepository = this.uow.Get<Acquirente, int>();
            this.allevamentiRepository = this.uow.Get<Allevamento, int>();
            this.utentiRepository = this.uow.Get<Utente, int>();

            this.service = this.scope.Resolve<IAnalisiMappaService>();

            this.dbCleaner = new DbCleaner(uow);
        }

        #endregion

        #region Methods

        [SetUp]
        public void Init()
        {
            var acquirente = new Acquirente();
            acquirente.RagioneSociale = "Latte Marche";
            acquirente = this.acquirentiRepository.Add(acquirente);
            this.uow.SaveChanges();

            var utente = new Utente();
            utente.RagioneSociale = "Allevamento 1";
            utente.IdTipoLatte = 6;
            utente.UtenteXAcquirente = new UtenteXAcquirente()
            {
                Acquirente = acquirente
            };
            this.utentiRepository.Add(utente);

            var allevamento = new Allevamento();
            allevamento.Utente = utente;
            allevamento.Latitudine = 43;
            allevamento.Longitudine = 13;
            this.allevamentiRepository.Add(allevamento);

            this.uow.SaveChanges();
        }

        [TearDown]
        public void CleanUp()
        {
            this.dbCleaner.CleanUp();
        }

        [Test]
        public void AnalisiMappaTest_Load()
        {
            var mapSearchDto = new MapSearchDto();
            var dto = this.service.Load(mapSearchDto);
            Assert.IsNotNull(dto);
        }


        #endregion
    }
}
