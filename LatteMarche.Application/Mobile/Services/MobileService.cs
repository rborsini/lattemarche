using LatteMarche.Application.Acquirenti.Interfaces;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.Destinatari.Interfaces;
using LatteMarche.Application.Latte.Interfaces;
using LatteMarche.Application.Mobile.Dtos;
using LatteMarche.Application.Mobile.Interfaces;
using LatteMarche.Application.Trasportatori.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Linq;

namespace LatteMarche.Application.Mobile.Services
{
    public class MobileService : IMobileService
    {
        #region Fields

        private IUnitOfWork uow;
        private IRepository<DispositivoMobile, string> dispositiviRepository;

        private ITrasportatoriService trasportatoriService;
        private IAutocisterneService autocisterneService;
        private IGiriService giriService;
        private IAllevamentiService allevamentiService;
        private IAcquirentiService acquirentiService;
        private IDestinatariService destinatariService;
        private ITipiLatteService tipiLatteService;
        private ILottiService lottiService;

        #endregion

        #region Constructor

        public MobileService(
            IUnitOfWork uow, 
            ITrasportatoriService trasportatoriService,
            IAutocisterneService autocisterneService,
            IGiriService giriService,
            IAllevamentiService allevamentiService,
            IAcquirentiService acquirentiService,
            IDestinatariService destinatariService,
            ITipiLatteService tipiLatteService,
            ILottiService lottiService
            )
        {
            this.uow = uow;

            this.dispositiviRepository = this.uow.Get<DispositivoMobile, string>();

            this.trasportatoriService = trasportatoriService;
            this.autocisterneService = autocisterneService;
            this.giriService = giriService;
            this.allevamentiService = allevamentiService;
            this.acquirentiService = acquirentiService;
            this.destinatariService = destinatariService;
            this.tipiLatteService = tipiLatteService;
            this.lottiService = lottiService;

        }

        #endregion

        public void Register(DeviceInfoDto deviceInfo)
        {
            if (String.IsNullOrEmpty(deviceInfo.IMEI))
                return;

            var dispositivo = this.dispositiviRepository.GetById(deviceInfo.IMEI);

            if(dispositivo == null)
            {
                // nuovo dispositivo
                dispositivo = new DispositivoMobile()
                {
                    DataRegistrazione = DateTime.Now,
                    Id = deviceInfo.IMEI,
                    Latitudine = deviceInfo.Lat,
                    Longitudine = deviceInfo.Lng,
                    VersioneApp = deviceInfo.VersioneApp
                };

                this.dispositiviRepository.Add(dispositivo);
                this.uow.SaveChanges();
            }            
        }

        public LocalDbDto Download(string imei)
        {
            LocalDbDto db = null;
            var dispositivo = this.dispositiviRepository.GetById(imei);

            if(dispositivo != null && dispositivo.Attivo)
            {
                db = new LocalDbDto();
                db.TipiLatte = this.tipiLatteService.Index();
                db.Acquirenti = this.acquirentiService.Index();
                db.Destinatari = this.destinatariService.Index();

                if(dispositivo.IdTrasportatore.HasValue)
                {
                    var idTrasportatore = dispositivo.IdTrasportatore.Value;

                    var giri = this.giriService.GetGiriTrasportatore(idTrasportatore);

                    foreach(var idGiro in giri.Select(g => g.Id))
                    {
                        var giro = this.giriService.Details(idGiro);

                        giro.Items = giro.Items.Where(i => i.Priorita.HasValue).ToList(); // prendo solo gli items con priorità
                        db.Giri.Add(giro);
                    }
                        

                    db.Trasportatore = this.trasportatoriService.Details(idTrasportatore);
                    db.Trasportatore.Giri.Clear();
                    db.Autocisterna = this.autocisterneService.Index().FirstOrDefault(a => a.IdTrasportatore == idTrasportatore);

                    var idAllevamenti = db.Giri.SelectMany(g => g.Items.Select(a => a.IdAllevamento)).ToList();

                    db.Allevamenti = this.allevamentiService.Index().Where(a => idAllevamenti.Contains(a.Id)).ToList();
                }

                dispositivo.DataUltimoDownload = DateTime.Now;
                this.dispositiviRepository.Update(dispositivo);
                this.uow.SaveChanges();
            }

            return db;
        }

        public void Upload(UploadDto uploadDto)
        {
            var dispositivo = this.dispositiviRepository.GetById(uploadDto.IMEI);

            if(dispositivo != null && dispositivo.Attivo)
            {
                foreach(var lotto in uploadDto.Lotti)
                    this.lottiService.Create(lotto);

                dispositivo.Latitudine = uploadDto.Lat;
                dispositivo.Longitudine = uploadDto.Lng;
                dispositivo.DataUltimoUpload = DateTime.Now;
                dispositivo.VersioneApp = uploadDto.VersioneApp;

                this.dispositiviRepository.Update(dispositivo);
                this.uow.SaveChanges();                
            }
        }


    }
}
