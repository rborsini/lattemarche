using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LatteMarche.Core;
using LatteMarche.Application;
using Autofac;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.EntityFramework;

namespace LatteMarche.Tests.Services
{
    [TestClass]
    public class GiriServiceTest
    {

        private IGiriService giriService;
        private IUnitOfWork uow;

        public GiriServiceTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();

            this.giriService = AutoFacConfig.Container.Resolve<IGiriService>();
            this.uow = AutoFacConfig.Container.Resolve<IUnitOfWork>();
        }

        [TestMethod]
        public void Index_SomeRecords_OrderedByCognome()
        {
            


        }

        [TestMethod]
        public void Update_NewRecord_Inserted()
        {



        }
    }
}
