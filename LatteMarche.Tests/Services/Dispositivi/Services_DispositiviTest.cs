using Autofac;
using FizzWare.NBuilder;
using LatteMarche.Application.Dispositivi.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Tests.Services.Dispositivi
{
    [TestFixture]
    public class Services_DispositiviTest
    {

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<DispositivoMobile, string> dispositiviRepository;

        private IDispositiviService dispositiviService;

        private DbCleaner dbCleaner;


        #endregion

        #region Constructor

        public Services_DispositiviTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = scope.Resolve<IUnitOfWork>();
            this.dbCleaner = new DbCleaner(uow);

            this.dispositiviRepository = this.uow.Get<DispositivoMobile, string>();

            this.dispositiviService = this.scope.Resolve<IDispositiviService>();

        }

        #endregion

        #region Methods

        [SetUp]
        public void Init()
        {

        }

        [TearDown]
        public void CleanUp()
        {
            this.dbCleaner.CleanUp();
        }

        [Test]
        public void DispositiviService_Search()
        {
            var deviceEntities = Builder<DispositivoMobile>
                                    .CreateListOfSize(10)
                                        .All()
                                            .With(d => d.IdTrasportatore = (int?)null)
                                    .Build();

            this.dispositiviRepository.Add(deviceEntities);
            this.uow.SaveChanges();

            var list = this.dispositiviService.Index();
            Assert.AreEqual(10, list.Count);

        }


        #endregion
    }
}
