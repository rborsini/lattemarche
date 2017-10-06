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
        private ILifetimeScope scope;

        private IGiriService giriService;
        private IUnitOfWork uow;
        private IRepository<V_Allevatore, int> allevatoriRepository;
        private IRepository<Giro, int> giriRepository;

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

        [TestMethod]
        public void Details_SomeRecords_OrderedByCognome()
        {
            List<V_Allevatore> allevatori = Builder<V_Allevatore>.CreateListOfSize(10)
               .All().Do(a => this.allevatoriRepository.Add(a))
               .Build().ToList();

            List<Giro> giri = Builder<Giro>.CreateListOfSize(2)
                .TheFirst(1).With(g => g.Id = 1)
                .TheNext(1).With(g => g.Id = 2)
                .All().Do(g => this.giriRepository.Add(g))
                .Build().ToList();

            List<AllevamentoXGiro> items = Builder<AllevamentoXGiro>.CreateListOfSize(10)
                .TheFirst(5).With(o => o.IdGiro = 1)
                .TheNext(5).With(o => o.IdGiro = 2)
                .All().Do(o => this.uow.Context.AllevamentiXGiro.Add(o))
                .Build().ToList();

            var obj = this.giriRepository.GetById(1);

            //this.uow.SaveChanges();

            GiroDto giro = this.giriService.Details(1);

            Assert.AreEqual(5, giro.Items.Count(i => i.Priorita.HasValue));

        }

        //[TestMethod]
        //public void Update_NewRecord_Inserted()
        //{



        //}
    }
}
