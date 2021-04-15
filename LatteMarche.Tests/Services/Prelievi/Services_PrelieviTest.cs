using Autofac;
using FizzWare.NBuilder;
using FluentAssertions;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Tests.Services.Prelievi
{
    [TestFixture]
    public class Services_PrelieviTest
    {
        #region Constants

        private const int ID_COMUNE = 252;      // Filottrano (unico comune presente in test)
        private const int ID_PROFILO_TRASPORTATORE = 5;         // Trasportatore
        private const int ID_PROFILO_ALLEVATORE = 3;            // Allevatore
        private const int ID_TIPO_LATTE_QM = 1;                 // QM Alta Qualità

        private const int ID_ACQUIRENTE_DEFAULT = 123;
        private const int ID_DESTINATARIO_DEFAULT = 321;

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<PrelievoLatte, int> prelieviRepository;

        private IPrelieviLatteService prelieviLatteService;

        private DbCleaner dbCleaner;

        #endregion

        #region Constructor

        public Services_PrelieviTest()
        {
            AutoFacConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();
            this.dbCleaner = new DbCleaner(uow);

            this.prelieviRepository = this.uow.Get<PrelievoLatte, int>();

            this.prelieviLatteService = this.scope.Resolve<IPrelieviLatteService>();
        }

        #endregion

        #region Init / TearDown Methods

        [SetUp]
        public void Init()
        {
            // prelievi
            var prelievi = new List<PrelievoLatte>();
            for (int i = 1; i <= 10; i++)
            {
                prelievi.Add(Builder<PrelievoLatte>
                    .CreateNew()
                        .With(p => p.IdTipoLatte = ID_TIPO_LATTE_QM)
                        .With(p => p.DataPrelievo = DateTime.Now)
                        .With(p => p.DataUltimaMungitura = DateTime.Now)
                    .Build()
                );
            }

            this.prelieviRepository.Add(prelievi);
            this.uow.SaveChanges();
        }

        [TearDown]
        public void CleanUp()
        {
            this.dbCleaner.CleanUp();
        }

        #endregion

        #region Test Methods

        [Test]
        public void PrelieviLatteService_Sitra()
        {
            var prelieviDaInviare = this.prelieviLatteService.Sitra(DateTime.Today);

            // il caricamento dai prelievi sitra non deve troncare l'ora del prelievo e della mungitura
            prelieviDaInviare.All(p => p.DataPrelievo.Value.Hour != 0).Should().BeTrue();
            prelieviDaInviare.All(p => p.DataUltimaMungitura.Value.Hour != 0).Should().BeTrue();
        }

        #endregion


    }
}
