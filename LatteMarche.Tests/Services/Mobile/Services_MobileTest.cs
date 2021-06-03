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
        private const int ID_TIPO_LATTE_QM = 1;                 // QM Alta Qualità
        private const long ID_TRASBORDO = 12;

        #endregion

        #region Fields

        private ILifetimeScope scope;

        private IUnitOfWork uow;

        private IRepository<DispositivoMobile, string> deviceRepository;
        private IRepository<PrelievoLatte, int> prelieviRepository;
        private IRepository<Utente, int> utentiRepository;
        private IRepository<Allevamento, int> allevamentiRepository;
        private IRepository<Giro, int> giriRepository;

        private IRepository<Autocisterna, int> autocisterneRepository;
        private IRepository<Acquirente, int> acquirentiRepository;
        private IRepository<Cessionario, int> cessionariRepository;
        private IRepository<Destinatario, int> destinatariRepository;

        private IMobileService mobileService;

        private DbCleaner dbCleaner;

        private int idAcquirenteDefault;
        private int idDestinatarioDefault;

        private List<Autocisterna> autocisterne;
        private Autocisterna autocisterna;
        private Acquirente acquirente;
        private Destinatario destinatario;
        private Utente trasportatore;
        private Utente allevatore;
        private Allevamento allevamento;

        #endregion

        #region Constructor

        public Services_MobileTest()
        {
            AutoFacConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();

            this.uow = this.scope.Resolve<IUnitOfWork>();
            this.dbCleaner = new DbCleaner(uow);
            
            this.prelieviRepository = this.uow.Get<PrelievoLatte, int>();

            this.deviceRepository = this.uow.Get<DispositivoMobile, string>();
            this.utentiRepository = this.uow.Get<Utente, int>();

            this.allevamentiRepository = this.uow.Get<Allevamento, int>();
            this.giriRepository = this.uow.Get<Giro, int>();

            this.autocisterneRepository = this.uow.Get<Autocisterna, int>();
            this.acquirentiRepository = this.uow.Get<Acquirente, int>();
            this.cessionariRepository = this.uow.Get<Cessionario, int>();
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

            // autocisterne
            this.autocisterne = Builder<Autocisterna>
                .CreateListOfSize(2)
                    .All()
                        .With(a => a.IdTrasportatore = this.trasportatore.Id)
                .Build()
                .ToList();

            this.autocisterneRepository.Add(this.autocisterne);
            this.autocisterna = autocisterne.First();

            // utente allevatore
            this.allevatore = Builder<Utente>
                .CreateNew()
                    .With(u => u.Id = 2)
                    .With(u => u.IdComune = ID_COMUNE)
                    .With(u => u.IdProfilo = ID_PROFILO_ALLEVATORE)
                    .With(u => u.IdTipoLatte = ID_TIPO_LATTE_QM)
                .Build();

            this.allevatore = this.utentiRepository.Add(allevatore);

            // allevamento
            this.allevamento = Builder<Allevamento>
                .CreateNew()
                    .With(a => a.IdUtente = this.allevatore.Id)
                    .With(u => u.IdComune = ID_COMUNE)
                .Build();

            this.allevamento = this.allevamentiRepository.Add(this.allevamento);

            // acquirente
            this.acquirente = Builder<Acquirente>
                .CreateNew()
                    .With(u => u.IdComune = ID_COMUNE)
                    .With(u => u.Abilitato = true)
                .Build();

            this.acquirentiRepository.Add(acquirente);            

            // cessionario
            var cessionario = Builder<Cessionario>
                .CreateNew()                    
                    .With(u => u.IdComune = ID_COMUNE)
                    .With(u => u.Abilitato = true)
                .Build();

            this.cessionariRepository.Add(cessionario);

            // destinatario
            this.destinatario = Builder<Destinatario>
                .CreateNew()
                    .With(u => u.IdComune = ID_COMUNE)
                    .With(u => u.Abilitato = true)
                .Build();

            this.destinatariRepository.Add(destinatario);

            this.uow.SaveChanges();

            this.idAcquirenteDefault = acquirente.Id;
            this.idDestinatarioDefault = destinatario.Id;
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

            var italyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");

            Assert.IsNotNull(deviceEntity);
            Assert.IsTrue((DateTime.Now - TimeZoneInfo.ConvertTimeFromUtc(deviceEntity.DataRegistrazione, italyTimeZone)).TotalSeconds < 10);
            
        }

        [Test]
        public void MobileService_Download()
        {
            var id = "ABCD";

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
                var idAcquirente = i % 4 == 0 ? i : this.acquirente.Id;
                var idDestinatario = i % 4 == 0 ? i : this.destinatario.Id;

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

            Assert.AreEqual(this.acquirente.Id, allevamentoDto.IdAcquirenteDefault);
            Assert.AreEqual(this.destinatario.Id, allevamentoDto.IdDestinatarioDefault);

            Assert.AreEqual(5.95, allevamentoDto.Quantita_Min);         // 5° percentile
            Assert.AreEqual(95.05, allevamentoDto.Quantita_Max);        // 5° percentile

            Assert.AreEqual(5.95, allevamentoDto.Temperatura_Min);      // 95° percentile
            Assert.AreEqual(95.05, allevamentoDto.Temperatura_Max);     // 95° percentile

            // cap not null Bug #325926
            Assert.IsNotNull(localDb.Acquirenti[0].CAP);
            Assert.IsNotNull(localDb.Destinatari[0].CAP);
            Assert.IsNotNull(localDb.Giri[0].Allevamenti[0].CAP);

            // #325953
            Assert.IsTrue(localDb.Cessionari.Count > 0);

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
                VersioneApp = "0.1",
                IdAutocisterna = this.autocisterna.Id
            };

            uploadDto.Prelievi = Builder<Application.Mobile.Dtos.PrelievoLatteDto>
                .CreateListOfSize(3)
                    .All()
                        .With(p => p.IdAllevamento = this.allevamento.Id)
                        .With(p => p.DataConsegna = DateTime.Now)
                        .With(p => p.DataPrelievo = DateTime.Now)
                        .With(p => p.DataUltimaMungitura = DateTime.Now)
                        .With(p => p.IdTrasbordo = ID_TRASBORDO)
                .Build()
                .ToList();

            this.mobileService.Upload(uploadDto);

            var prelievi = this.prelieviRepository.Query.ToList();
            Assert.AreEqual(3, prelievi.Count);

            Assert.AreEqual(ID_TIPO_LATTE_QM, prelievi[0].IdTipoLatte.Value);
            Assert.AreEqual(ID_TRASBORDO, prelievi[0].IdTrasbordo.Value);

            Assert.AreEqual(imei, prelievi.First().DeviceId);

            deviceEntity = this.deviceRepository.GetById(imei);
            Assert.IsTrue(deviceEntity.DataUltimoUpload.HasValue);
        }

        #endregion
    }
}
