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
    public class Services_UtentiTest
    {

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<Utente, int> utentiRepository;
        private IRepository<TipoProfilo, int> profiliRepository;
        private IUtentiService utentiService;

        private DbCleaner dbCleaner;

        #endregion

        #region Constructor

        public Services_UtentiTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();
            this.dbCleaner = new DbCleaner(uow);

            this.profiliRepository = this.uow.Get<TipoProfilo, int>();
            this.utentiRepository = this.uow.Get<Utente, int>();
            this.utentiService = this.scope.Resolve<IUtentiService>();
        }

        #endregion

        #region Methods

        [SetUp]
        public void Init()
        {
            var profiliIds = this.profiliRepository.DbSet.Select(p => p.Id).ToList();

            Random rnd = new Random();
            var utenti = Builder<Utente>
                        .CreateListOfSize(100)
                        .TheFirst(50)
                            .With(u => u.Abilitato = true)
                        .TheRest()
                            .With(u => u.Abilitato = false)
                        .All()
                            .With(d => d.IdComune = (int?)null)
                            .With(d => d.IdProfilo = profiliIds[rnd.Next(0, profiliIds.Count - 1)])
                        .Build();

            this.utentiRepository.Add(utenti);
            this.uow.SaveChanges();
        }

        [TearDown]
        public void CleanUp()
        {
            this.dbCleaner.CleanUp();
        }

        [Test]
        public void UtentiService_Search()
        {
            var searchDto = new UtentiSearchDto();

            // allevatore => 3
            searchDto.IdProfilo = (int)ProfiloEnum.Allevatore;
            searchDto.Length = -1;
            var utentiDto = this.utentiService.Search(searchDto).FilteredList;

            Assert.IsTrue(utentiDto.All(u => u.IdProfilo == (int)ProfiloEnum.Allevatore));

            var allevatoriCount = this.utentiRepository.DbSet.Count(u => u.Abilitato && u.IdProfilo == (int)ProfiloEnum.Allevatore);

            Assert.AreEqual(allevatoriCount, utentiDto.Count);

        }


        #endregion

    }
}
