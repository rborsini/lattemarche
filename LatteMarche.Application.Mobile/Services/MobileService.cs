using AutoMapper;
using LatteMarche.Application.Mobile.Dtos;
using LatteMarche.Application.Mobile.Interfaces;
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

        private IRepository<V_Trasportatore, int> trasportatoriRepository;
        private IRepository<Autocisterna, int> autocisterneRepository;
        private IRepository<Giro, int> giriRepository;
        private IRepository<TipoLatte, int> tipiLatteRepository;
        private IRepository<Acquirente, int> acquirentiRepository;
        private IRepository<Destinatario, int> destinataryRepository;

        #endregion

        #region Constructor

        public MobileService(IUnitOfWork uow)
        {
            this.uow = uow;

            this.dispositiviRepository = this.uow.Get<DispositivoMobile, string>();

            this.trasportatoriRepository = this.uow.Get<V_Trasportatore, int>();
            this.autocisterneRepository = this.uow.Get<Autocisterna, int>();
            this.giriRepository = this.uow.Get<Giro, int>();

            this.tipiLatteRepository = this.uow.Get<TipoLatte, int>();
            this.acquirentiRepository = this.uow.Get<Acquirente, int>();
            this.destinataryRepository = this.uow.Get<Destinatario, int>();

        }

        #endregion

        #region Methods


        public DispositivoDto Register(DispositivoDto deviceInfo)
        {
            if (String.IsNullOrEmpty(deviceInfo.Id))
                return null;

            var dispositivo = this.dispositiviRepository.GetById(deviceInfo.Id);

            if (dispositivo == null)
            {
                // nuovo dispositivo
                dispositivo = Mapper.Map<DispositivoMobile>(deviceInfo);
                dispositivo.DataRegistrazione = DateTime.Now;

                this.dispositiviRepository.Add(dispositivo);
                this.uow.SaveChanges();
            }

            return Mapper.Map<DispositivoDto>(this.dispositiviRepository.GetById(deviceInfo.Id));
        }

        public DownloadDto Download(string imei)
        {
            DownloadDto db = null;
            var dispositivo = this.dispositiviRepository.GetById(imei);

            if (dispositivo != null && dispositivo.Attivo)
            {
                db = new DownloadDto();

                if (dispositivo.IdTrasportatore.HasValue)
                {
                    var idTrasportatore = dispositivo.IdTrasportatore.Value;

                    db.Trasportatore = Mapper.Map<TrasportatoreDto>(this.trasportatoriRepository.GetById(idTrasportatore));
                    db.Autocisterna = Mapper.Map<AutocisternaDto>(this.autocisterneRepository.DbSet.FirstOrDefault(a => a.IdTrasportatore == idTrasportatore));
                    db.Giri = Mapper.Map<List<TemplateGiroDto>>(this.giriRepository.DbSet.Where(g => g.IdTrasportatore == idTrasportatore).ToList());

                    foreach (var giro in db.Giri)
                    {
                        var giroFull = this.giriRepository.GetById(giro.Id);

                        var allevamentiXGiro = giroFull.Allevamenti.Where(a => a.Priorita.HasValue).ToList();

                        foreach (var axg in allevamentiXGiro)
                        {
                            var allevamento = axg.Allevamento;
                            var utente = allevamento != null ? allevamento.Utente : null;
                            var comune = utente != null ? utente.Comune : null;

                            giro.Allevamenti.Add(new AllevamentoDto()
                            {
                                IdAllevamento = allevamento.Id,
                                IdTemplateGiro = giro.Id,
                                IdTipoLatte = utente != null ? utente.IdTipoLatte : (int?)null,
                                Indirizzo = allevamento != null ? allevamento.IndirizzoAllevamento.Trim() : "",
                                Comune = comune != null ? comune.Descrizione.Trim() : "",
                                Provincia = comune != null ? comune.Provincia : "",
                                Priorita = axg.Priorita,
                                P_IVA = utente != null ? utente.PivaCF.Trim() : "",
                                RagioneSociale = utente != null ? utente.RagioneSociale.Trim() : ""
                            });
                        }
                    }

                    db.TipiLatte = Mapper.Map<List<TipoLatteDto>>(this.tipiLatteRepository.GetAll());
                    db.Acquirenti = Mapper.Map<List<AcquirenteDto>>(this.acquirentiRepository.GetAll().ToList());
                    db.Destinatari = Mapper.Map<List<DestinatarioDto>>(this.destinataryRepository.GetAll().ToList());

                }

                dispositivo.DataUltimoDownload = DateTime.Now;
                this.dispositiviRepository.Update(dispositivo);
                this.uow.SaveChanges();
            }

            return db;
        }

        public void Upload(UploadDto uploadDto)
        {
            //    var dispositivo = this.dispositiviRepository.GetById(uploadDto.IMEI);

            //    if (dispositivo != null && dispositivo.Attivo)
            //    {
            //        foreach (var prelievo in uploadDto.Prelievi)
            //        {
            //            this.prelieviLatteService.Create(prelievo);
            //        }

            //        dispositivo.Latitudine = uploadDto.Lat;
            //        dispositivo.Longitudine = uploadDto.Lng;
            //        dispositivo.DataUltimoUpload = DateTime.Now;
            //        dispositivo.VersioneApp = uploadDto.VersioneApp;

            //        this.dispositiviRepository.Update(dispositivo);
            //        this.uow.SaveChanges();
            //    }
        }

        #endregion

    }
}
