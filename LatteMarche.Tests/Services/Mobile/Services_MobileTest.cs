using Autofac;
using FizzWare.NBuilder;
using LatteMarche.Application.Latte.Dtos;
using LatteMarche.Application.Mobile.Dtos;
using LatteMarche.Application.Mobile.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Tests.Services.Mobile
{
    [TestFixture]
    public class Services_MobileTest
    {
        #region Constants

        private const int ID_COMUNE = 252;      // Filottrano (unico comune presente in test)
        private const int ID_PROFILO = 5;       // Trasportatore

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<DispositivoMobile, string> deviceRepository;
        private IRepository<Lotto, long> lottiRepository;
        private IRepository<PrelievoLatte, int> prelieviRepository;
        private IRepository<Utente, int> utentiRepository;

        private IMobileService mobileService;

        private DbCleaner dbCleaner;

        private Utente utente;

        #endregion

        #region Constructor

        public Services_MobileTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();
            this.dbCleaner = new DbCleaner(uow);

            this.lottiRepository = this.uow.Get<Lotto, long>();
            this.prelieviRepository = this.uow.Get<PrelievoLatte, int>();

            this.deviceRepository = this.uow.Get<DispositivoMobile, string>();
            this.utentiRepository = this.uow.Get<Utente, int>();

            this.mobileService = this.scope.Resolve<IMobileService>();
        }

        #endregion

        #region Methods

        [SetUp]
        public void Init()
        {
            this.utente = Builder<Utente>
                .CreateNew()
                    .With(u => u.IdComune = ID_COMUNE)
                    .With(u => u.IdProfilo = ID_PROFILO)
                .Build();

            this.utente = this.utentiRepository.Add(utente);
            this.uow.SaveChanges();
        }

        [TearDown]
        public void CleanUp()
        {
            this.dbCleaner.CleanUp();
        }

        [Test]
        public void MobileService_Register()
        {
            var deviceInfoDto = Builder<DispositivoDto>
                .CreateNew()
                    .With(d => d.IdTrasportatore = (int?)null)
                .Build();

            deviceInfoDto = this.mobileService.Register(deviceInfoDto);

            Assert.IsNotNull(deviceInfoDto);

            var deviceEntity = this.deviceRepository.GetById(deviceInfoDto.Id);

            Assert.IsNotNull(deviceEntity);
            Assert.IsTrue((DateTime.Now - deviceEntity.DataRegistrazione).TotalSeconds < 10);
            
        }

        [Test]
        public void MobileService_Download()
        {
            var imei = "ABCD";

            var deviceEntity = Builder<DispositivoMobile>
                .CreateNew()
                    .With(d => d.Id = imei)
                    .With(d => d.Attivo = true)
                    .With(d => d.IdTrasportatore = this.utente.Id)
                .Build();

            this.deviceRepository.Add(deviceEntity);
            this.uow.SaveChanges();

            var localDb = this.mobileService.Download(imei);
            Assert.IsNotNull(localDb);
            Assert.IsNotNull(localDb.Trasportatore);

            deviceEntity = this.deviceRepository.GetById(imei);
            Assert.IsTrue(deviceEntity.DataUltimoDownload.HasValue);

        }

        [Test]
        public void MobileService_Upload()
        {
            var imei = "ABCD";

            var deviceEntity = Builder<DispositivoMobile>
                .CreateNew()
                    .With(d => d.Id = imei)
                    .With(d => d.Attivo = true)
                    .With(d => d.IdTrasportatore = this.utente.Id)
                .Build();

            this.deviceRepository.Add(deviceEntity);
            this.uow.SaveChanges();

            var uploadDto = new UploadDto()
            {
                IMEI = imei,
                Lat = 12,
                Lng = 42,
                VersioneApp = "0.1"
            };

            uploadDto.Prelievi = Builder<Application.Mobile.Dtos.PrelievoLatteDto>
                .CreateListOfSize(3)
                    .All()
                        .With(p => p.DataConsegna = DateTime.Now)
                        .With(p => p.DataPrelievo = DateTime.Now)
                        .With(p => p.DataUltimaMungitura = DateTime.Now)
                .Build()
                .ToList();

            this.mobileService.Upload(uploadDto);

            var prelievi = this.prelieviRepository.Query.ToList();
            Assert.AreEqual(3, prelievi.Count);

            deviceEntity = this.deviceRepository.GetById(imei);
            Assert.IsTrue(deviceEntity.DataUltimoUpload.HasValue);
        }

        #endregion
    }
}
