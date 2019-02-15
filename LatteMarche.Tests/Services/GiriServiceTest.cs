using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LatteMarche.Core;
using LatteMarche.Application;
using Autofac;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.EntityFramework;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using LatteMarche.Application.Giri.Dtos;

namespace LatteMarche.Tests.Services
{
    [TestClass]
    public class GiriServiceTest
    {

        #region Fields

        private ILifetimeScope scope;

        private IGiriService giriService;
        private IUnitOfWork uow;
        private IRepository<V_Allevatore, int> allevatoriRepository;
        private IRepository<Giro, int> giriRepository;

        #endregion

        #region Constructors

        public GiriServiceTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.giriService = this.scope.Resolve<IGiriService>();
            this.uow = this.scope.Resolve<IUnitOfWork>();
            this.giriRepository = this.uow.Get<Giro, int>();
            this.allevatoriRepository = this.uow.Get<V_Allevatore, int>();
        }

        #endregion

        #region Methods

        [TestMethod]
        public void GiriService_Details_AlcuniRecordInseriti_AllevatoriOrdinatiAlfabeticamente()
        {
            this.SeedAllevatori(9);
            this.SeedGiri(1, 2);
            this.SeedAllevamentiXGiro(9, 1, 2);

            GiroDto giro = this.giriService.Details(1);

            Assert.AreEqual(9/2, giro.Items.Count(i => i.Priorita.HasValue));

            Assert.IsTrue(String.Compare(giro.Items[0].Allevatore, giro.Items[1].Allevatore) < 0);
            Assert.IsTrue(String.Compare(giro.Items[1].Allevatore, giro.Items[2].Allevatore) < 0);

        }

        [TestMethod]
        public void GiriService_Update_PrioritaSelezionate_NuoviRecordInseriti()
        {
            this.SeedAllevatori(9);
            this.SeedGiri(1, 2);

            GiroDto giro = this.giriService.Details(1);

            giro.Items[0].Priorita = 1;
            giro.Items[1].Priorita = 2;

            giro = this.giriService.Update(giro);

            Assert.AreEqual(2, this.uow.Context.AllevamentiXGiro.Count());
            Assert.AreEqual(2, giro.Items.Count(i => i.Priorita.HasValue));
        }

        [TestMethod]
        public void GiriService_Update_PrioritaDeselezionate_RecordEliminati()
        {
            this.SeedAllevatori(9);
            this.SeedGiri(1);
            this.SeedAllevamentiXGiro(9, 1, 2);

            GiroDto giro = this.giriService.Details(1);

            foreach(GiroItemDto item in giro.Items)
            {
                item.Priorita = (int?)null;
            }

            giro = this.giriService.Update(giro);

            Assert.AreEqual(9, giro.Items.Count);
            Assert.AreEqual(5, this.uow.Context.AllevamentiXGiro.Count());      // priorità del giro 2
            Assert.AreEqual(0, giro.Items.Count(i => i.Priorita.HasValue));
        }


        private void SeedAllevatori(int length)
        {
            List<V_Allevatore> allevatori = Builder<V_Allevatore>.CreateListOfSize(length)
                 .All().Do(a => this.allevatoriRepository.Add(a))
                 .Build().ToList();
        }

        private void SeedGiri(params int[] ids)
        {
            foreach(int id in ids)
            {
                Builder<Giro>.CreateNew()
                    .With(g => g.Id = id)
                    .Do(g => this.giriRepository.Add(g))
                    .Build();
            }
        }

        private void SeedAllevamentiXGiro(int length, int id1, int id2)
        {
            List<AllevamentoXGiro> items = Builder<AllevamentoXGiro>.CreateListOfSize(length)
                .TheFirst(length / 2).With(o => o.IdGiro = id1)
                .TheNext(length - (length/2)).With(o => o.IdGiro = id2)
                .All().Do(o => this.uow.Context.AllevamentiXGiro.Add(o))
                .Build().ToList();
        }

        #endregion

    }
}
