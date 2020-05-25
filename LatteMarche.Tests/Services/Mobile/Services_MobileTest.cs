using Autofac;
using FizzWare.NBuilder;
using LatteMarche.Application.Mobile.Dtos;
using LatteMarche.Application.Mobile.Interfaces;
using LatteMarche.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WeCode.Data.Interfaces;

namespace LatteMarche.Tests.Services.Mobile
{
    [TestFixture]
    public class Services_MobileTest
    {
        #region Constants

        private const int ID_COMUNE = 252;      // Filottrano (unico comune presente in test)
        private const int ID_PROFILO_TRASPORTATORE = 5;         // Trasportatore
        private const int ID_PROFILO_ALLEVATORE = 3;            // Allevatore

        private const int ID_ACQUIRENTE_DEFAULT = 123;
        private const int ID_DESTINATARIO_DEFAULT = 321;

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<DispositivoMobile, string> deviceRepository;
        private IRepository<PrelievoLatte, int> prelieviRepository;
        private IRepository<Utente, int> utentiRepository;
        private IRepository<Allevamento, int> allevamentiRepository;
        private IRepository<Giro, int> giriRepository;

        private IRepository<Acquirente, int> acquirentiRepository;
        private IRepository<Destinatario, int> destinatariRepository;

        private IMobileService mobileService;

        private DbCleaner dbCleaner;

        private Utente trasportatore;

        #endregion

        #region Constructor

        public Services_MobileTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();
            this.dbCleaner = new DbCleaner(uow);
            
            this.prelieviRepository = this.uow.Get<PrelievoLatte, int>();

            this.deviceRepository = this.uow.Get<DispositivoMobile, string>();
            this.utentiRepository = this.uow.Get<Utente, int>();

            this.allevamentiRepository = this.uow.Get<Allevamento, int>();
            this.giriRepository = this.uow.Get<Giro, int>();

            this.acquirentiRepository = this.uow.Get<Acquirente, int>();
            this.destinatariRepository = this.uow.Get<Destinatario, int>();

            this.mobileService = this.scope.Resolve<IMobileService>();
        }

        #endregion

        #region Methods

        [SetUp]
        public void Init()
        {
            // utente trasportatore
            this.trasportatore = Builder<Utente>
                .CreateNew()
                    .With(u => u.Id = 1)
                    .With(u => u.IdComune = ID_COMUNE)
                    .With(u => u.IdProfilo = ID_PROFILO_TRASPORTATORE)
                .Build();

            this.trasportatore = this.utentiRepository.Add(trasportatore);

            // acquirente
            var acquirente = Builder<Acquirente>
                .CreateNew()
                    .With(u => u.IdComune = ID_COMUNE)
                .Build();

            this.acquirentiRepository.Add(acquirente);

            // destinatario
            var destinatario = Builder<Destinatario>
                .CreateNew()
                    .With(u => u.IdComune = ID_COMUNE)
                .Build();

            this.destinatariRepository.Add(destinatario);
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
                    .With(d => d.IdAutocisterna = (int?)null)
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
            var id = "ABCD";

            // utente allevatore
            var allevatore = Builder<Utente>
                .CreateNew()
                    .With(u => u.Id = 2)
                    .With(u => u.IdComune = ID_COMUNE)
                    .With(u => u.IdProfilo = ID_PROFILO_ALLEVATORE)
                .Build();

            allevatore = this.utentiRepository.Add(allevatore);
            this.uow.SaveChanges();

            // allevamento
            var allevamento = Builder<Allevamento>
                .CreateNew()
                    .With(a => a.IdComune = ID_COMUNE)
                    .With(a => a.IdUtente = allevatore.Id)
                    .With(a => a.Latitudine = 43.56854)
                    .With(a => a.Longitudine = 13.36993)
                .Build();

            allevamento = this.allevamentiRepository.Add(allevamento);
            this.uow.SaveChanges();

            // Giro
            var giro = Builder<Giro>
                .CreateNew()
                    .With(g => g.IdTrasportatore = this.trasportatore.Id)
                    .With(g => g.Allevamenti = new List<AllevamentoXGiro>() { new AllevamentoXGiro() { IdAllevamento = allevamento.Id, Priorita = 1 } })
                .Build();

            giro = this.giriRepository.Add(giro);
            this.uow.SaveChanges();

            // prelievi
            var prelievi = new List<PrelievoLatte>();
            for (int i = 1; i <= 100; i++)
            {
                var idAcquirente = i % 4 == 0 ? i : ID_ACQUIRENTE_DEFAULT;
                var idDestinatario = i % 4 == 0 ? i : ID_DESTINATARIO_DEFAULT;

                prelievi.Add(Builder<PrelievoLatte>
                    .CreateNew()
                        .With(p => p.IdAllevamento = allevamento.Id)
                        .With(p => p.IdAcquirente = idAcquirente)
                        .With(p => p.IdDestinatario = idDestinatario)
                        .With(p => p.Quantita = i)
                        .With(p => p.Temperatura = i)
                    .Build()
                );
            }

            this.prelieviRepository.Add(prelievi);
            this.uow.SaveChanges();

            var deviceEntity = Builder<DispositivoMobile>
                .CreateNew()
                    .With(d => d.Id = id)
                    .With(d => d.Attivo = true)
                    .With(d => d.IdTrasportatore = this.trasportatore.Id)
                    .With(d => d.IdAutocisterna = (int?)null)
                .Build();

            this.deviceRepository.Add(deviceEntity);
            this.uow.SaveChanges();

            var localDb = this.mobileService.Download(id);
            Assert.IsNotNull(localDb);
            Assert.IsNotNull(localDb.Trasportatore);
            Assert.AreEqual(1, localDb.Giri.Count);
            Assert.AreEqual(1, localDb.Giri[0].Allevamenti.Count);

            var allevamentoDto = localDb.Giri[0].Allevamenti[0];
            Assert.IsNotNull(allevamentoDto.Latitudine);
            Assert.IsNotNull(allevamentoDto.Longitudine);

            Assert.AreEqual(ID_ACQUIRENTE_DEFAULT, allevamentoDto.IdAcquirenteDefault);
            Assert.AreEqual(ID_DESTINATARIO_DEFAULT, allevamentoDto.IdDestinatarioDefault);

            Assert.AreEqual(5.95, allevamentoDto.Quantita_Min);         // 5° percentile
            Assert.AreEqual(95.05, allevamentoDto.Quantita_Max);        // 5° percentile

            Assert.AreEqual(5.95, allevamentoDto.Temperatura_Min);      // 95° percentile
            Assert.AreEqual(95.05, allevamentoDto.Temperatura_Max);     // 95° percentile

            // cap not null Bug #325926
            Assert.IsNotNull(localDb.Acquirenti[0].CAP);
            Assert.IsNotNull(localDb.Destinatari[0].CAP);
            Assert.IsNotNull(localDb.Giri[0].Allevamenti[0].CAP);

            deviceEntity = this.deviceRepository.GetById(id);
            Assert.IsTrue(deviceEntity.DataUltimoDownload.HasValue);

        }

        [Test]
        public void MobileService_Upload()
        {
            var imei = "DCBA";

            var deviceEntity = Builder<DispositivoMobile>
                .CreateNew()
                    .With(d => d.Id = imei)
                    .With(d => d.Attivo = true)
                    .With(d => d.IdTrasportatore = this.trasportatore.Id)
                    .With(d => d.IdAutocisterna = (int?)null)
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
