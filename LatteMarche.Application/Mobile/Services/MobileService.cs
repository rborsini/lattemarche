using LatteMarche.Application.Acquirenti.Interfaces;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.Autocisterne.Interfaces;
using LatteMarche.Application.Destinatari.Interfaces;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.Application.Lotti.Interfaces;
using LatteMarche.Application.Mobile.Dtos;
using LatteMarche.Application.Mobile.Interfaces;
using LatteMarche.Application.TipiLatte.Interfaces;
using LatteMarche.Application.Trasportatori.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                if(dispositivo.IdGiro.HasValue)
                {
                    db.Giro = this.giriService.Details(dispositivo.IdGiro.Value);
                    db.Giro.Items = db.Giro.Items.Where(i => i.Priorita.HasValue).ToList(); // prendo solo gli items con priorità
                    db.Trasportatore = this.trasportatoriService.Details(db.Giro.IdTrasportatore);
                    db.Trasportatore.Giri.Clear();
                    db.Autocisterna = this.autocisterneService.Index().FirstOrDefault(a => a.IdTrasportatore == db.Giro.IdTrasportatore);

                    var idAllevamenti = db.Giro.Items.Select(a => a.IdAllevamento).ToList();

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
                this.lottiService.Create(uploadDto.Lotto);

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
