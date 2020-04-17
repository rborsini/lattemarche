using Autofac;
using LatteMarche.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Tests.Services.Allevamenti
{
    [TestFixture]
    public class Services_AllevamentiTest
    {
        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        #endregion

        #region Constructor

        public Services_AllevamentiTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

        }

        #endregion

        #region Methods

        [SetUp]
        public void Init()
        {
            //var reviser = this.rolesRepository.DbSet.Where(r => r.Code == "Riesaminatore offerte").FirstOrDefault();

            //// 10 users
            //var users = Builder<User>
            //    .CreateListOfSize(10)
            //    .All()
            //        .With(u => u.Active = true)
            //        .With(u => u.UserRoles = new List<UserRole>() {
            //            Builder<UserRole>.CreateNew()
            //            .With(ur => ur.Username = u.Id)
            //            .With(ur => ur.RoleId = reviser.Id)
            //            .Build()
            //        })
            //    .Build();

            //this.usersRepository.Add(users);
            //this.uow.SaveChanges();
        }

        [TearDown]
        public void CleanUp()
        {
            //this.dbCleaner.CleanUp();
        }

        [Test]
        public void AllevamentiService_Search()
        {
            //// 10 offers (autore 'pippo' e 'pluto')
            //var offers = Builder<Offer>
            //    .CreateListOfSize(10)
            //    .All()
            //        .With(o => o.DocumentType = DocumentType.Offer)
            //        .With(o => o.LastChangeTimestmap = DateTime.Now)
            //        .With(o => o.StatusId = Convert.ToInt64(OfferStatus.Bozza))
            //        .With(o => o.DocumentType = DocumentType.Offer)
            //        .With(o => o.IsClosed = ClosedType.Open)
            //    .TheFirst(5)
            //        .With(o => o.Author = "pippo")
            //        .With(o => o.CustomerId = 32)        // Cliente esistente KALORGAS S.P.A.
            //        .With(o => o.HeadQuarterId = 0)                 //MONTEROBERTO
            //        .With(o => o.BusinessSublineId = "710000000")   //GEN - ERBUSCO
            //    .TheNext(2)
            //        .With(o => o.Code = "123-8678-19001")
            //        .With(o => o.Author = "pluto")
            //        .With(o => o.IsClosed = ClosedType.PartiallyClosed)
            //    .TheRest()
            //        .With(o => o.Author = "pluto")
            //    .Build();

            //this.offersRepository.Add(offers);
            //this.uow.SaveChanges();

            //// ricerca senza parametri => ritorna tutte le offerte
            //var list = this.offersService.Search().FilteredList;
            //Assert.AreEqual(0, list.Count);

            //// ricerca per cliente
            //list = this.offersService.Search(new OffersSearchDto() { CustomerId = 32 }).FilteredList;
            //Assert.AreEqual(5, list.Count);
            //Assert.AreEqual("KALORGAS S.P.A.", list[0].Customer_BusinessName);

            //// ricerca per sede
            //list = this.offersService.Search(new OffersSearchDto() { BusinessSublineId = "710000000" }).FilteredList;
            //Assert.AreEqual(5, list.Count);
            //Assert.AreEqual("GEN - ERBUSCO", list[0].BusinessSubline_Description);

            //// ricerca per sottolinea
            //list = this.offersService.Search(new OffersSearchDto() { HeadQuarterId = 0 }).FilteredList;
            //Assert.AreEqual(5, list.Count);
            //Assert.AreEqual("MONTEROBERTO", list[0].HeadQuarter_Description);


            //// ricerca per autore
            //list = this.offersService.Search(new OffersSearchDto() { Author = "pippo" }).FilteredList;
            //Assert.AreEqual(5, list.Count);

            //// ricerca per code
            //list = this.offersService.Search(new OffersSearchDto() { Code = "8678" }).FilteredList;
            //Assert.AreEqual(2, list.Count);

            //// ricerca per code
            //List<int> closedStatuses = new List<int>();
            //closedStatuses.Add(Convert.ToInt32(ClosedType.PartiallyClosed));
            //list = this.offersService.Search(new OffersSearchDto() { ClosedStatuses = closedStatuses }).FilteredList;
            //Assert.AreEqual(2, list.Count);

        }

        #endregion

    }
}
