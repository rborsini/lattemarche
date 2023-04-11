using AutoMapper;
using LatteMarche.Application.Mobile.Dtos;
using LatteMarche.Application.Mobile.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Mobile.Services
{
    public class MobileService : IMobileService
    {

        #region Fields

        private IUnitOfWork uow;
        private IMapper mapper;

        private IRepository<DispositivoMobile, string> dispositiviRepository;

        private IRepository<Utente, int> trasportatoriRepository;
        private IRepository<Autocisterna, int> autocisterneRepository;
        private IRepository<Giro, int> giriRepository;
        private IRepository<TipoLatte, int> tipiLatteRepository;
        private IRepository<Acquirente, int> acquirentiRepository;
        private IRepository<Destinatario, int> destinatariRepository;
        private IRepository<Cessionario, int> cessionariRepository;
        private IRepository<PrelievoLatte, int> prelieviRepository;
        private IRepository<Allevamento, int> allevamentiRepository;
        private IRepository<Utente, int> utentiRepository;

        #endregion

        #region Constructor

        public MobileService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;

            this.dispositiviRepository = this.uow.Get<DispositivoMobile, string>();

            this.trasportatoriRepository = this.uow.Get<Utente, int>();
            this.autocisterneRepository = this.uow.Get<Autocisterna, int>();
            this.giriRepository = this.uow.Get<Giro, int>();

            this.tipiLatteRepository = this.uow.Get<TipoLatte, int>();
            this.acquirentiRepository = this.uow.Get<Acquirente, int>();
            this.cessionariRepository = this.uow.Get<Cessionario, int>();
            this.destinatariRepository = this.uow.Get<Destinatario, int>();
            this.prelieviRepository = this.uow.Get<PrelievoLatte, int>();
            this.allevamentiRepository = this.uow.Get<Allevamento, int>();
            this.utentiRepository = this.uow.Get<Utente, int>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registrazione device
        /// </summary>
        /// <param name="dispostivo"></param>
        /// <returns></returns>
        public DispositivoDto Register(DispositivoDto deviceInfo)
        {
            if (String.IsNullOrEmpty(deviceInfo.Id))
                return null;

            var dispositivo = this.dispositiviRepository.GetById(deviceInfo.Id);

            if (dispositivo == null)
            {
                // nuovo dispositivo
                dispositivo = this.mapper.Map<DispositivoMobile>(deviceInfo);
                dispositivo.DataRegistrazione = DateTime.UtcNow;

                this.dispositiviRepository.Add(dispositivo);
                this.uow.SaveChanges();
            }
            else
            {
                dispositivo.VersioneApp = deviceInfo.VersioneApp;
                dispositivo.VersioneOS = deviceInfo.VersioneOS;
                dispositivo.Marca = deviceInfo.Marca;
                dispositivo.Modello = deviceInfo.Modello;

                if(deviceInfo.Lat.HasValue)
                    dispositivo.Latitudine = deviceInfo.Lat;

                if(deviceInfo.Lng.HasValue)
                    dispositivo.Longitudine = deviceInfo.Lng;

                if (deviceInfo.IdAutocisterna.HasValue)
                    dispositivo.IdAutocisterna = deviceInfo.IdAutocisterna;                

                this.dispositiviRepository.Update(dispositivo);
                this.uow.SaveChanges();
            }

            var dto = this.mapper.Map<DispositivoDto>(this.uow.Get<DispositivoMobile,string>().GetById(deviceInfo.Id));

            PushNotificationsService.Instance.Push(dto.Id);

            return dto;
        }

        /// <summary>
        /// Scaricamento dati di anagrafica e lookup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DownloadDto Download(string id)
        {
            var dispositivo = this.dispositiviRepository.GetById(id);
            if (dispositivo == null || !dispositivo.Attivo)
                return null;

            var db = new DownloadDto();

            if (dispositivo.IdTrasportatore.HasValue)
            {
                var idTrasportatore = dispositivo.IdTrasportatore.Value;

                db.Trasportatore = this.mapper.Map<TrasportatoreDto>(this.trasportatoriRepository.GetById(idTrasportatore));
                db.Autocisterna = this.mapper.Map<AutocisternaDto>(GetAutocisterna(dispositivo));
                db.Autocisterne = this.mapper.Map<List<AutocisternaDto>>(GetAutocisterne(dispositivo));
                db.Giri = this.mapper.Map<List<TemplateGiroDto>>(this.giriRepository.DbSet.Where(g => g.IdTrasportatore == idTrasportatore).ToList());
                db.TipiLatte = this.mapper.Map<List<TipoLatteDto>>(this.tipiLatteRepository.Query);

                var idTipiLatte = db.TipiLatte.Select(tl => tl.Id).ToList();
                var prelieviAutocisterna = GetPrelieviAutocisterna(db.Autocisterna.Id, 1000);

                foreach (var giro in db.Giri)
                {
                    var giroFull = this.giriRepository.GetById(giro.Id);

                    var allevamentiXGiro = giroFull.Allevamenti.Where(a => a.Priorita.HasValue && a.Allevamento.IdUtente.HasValue).ToList();

                    foreach (var axg in allevamentiXGiro)
                    {
                        var allevamento = axg.Allevamento;
                        var utente = allevamento != null ? allevamento.Utente : null;
                        var comune = utente != null ? utente.Comune : null;

                        var prelievi = prelieviAutocisterna.Where(p => p.IdAllevamento == allevamento.Id).ToList();
                        var temperature = prelievi.Where(p => p.Temperatura.HasValue).Select(p => p.Temperatura.Value).ToArray();
                        var quantita = prelievi.Where(p => p.Quantita.HasValue).Select(p => p.Quantita.Value).ToArray();

                        giro.Allevamenti.Add(new AllevamentoDto()
                        {
                            IdAllevamento = allevamento.Id,
                            IdTemplateGiro = giro.Id,
                            IdTipoLatte = utente != null && idTipiLatte.Contains(utente.IdTipoLatte) ? utente.IdTipoLatte : (int?)null,
                            Indirizzo = allevamento != null ? allevamento.IndirizzoAllevamento.Trim() : "",
                            CAP = comune != null ? comune.CAP.Trim() : "",
                            Comune = comune != null ? comune.Descrizione.Trim() : "",
                            Provincia = comune != null ? comune.Provincia : "",
                            Priorita = axg.Priorita,
                            P_IVA = utente != null && !String.IsNullOrEmpty(utente.PivaCF) ? utente.PivaCF.Trim() : "",
                            RagioneSociale = utente != null ? utente.RagioneSociale.Trim() : "",
                            Latitudine = allevamento?.Latitudine,
                            Longitudine = allevamento?.Longitudine,
                            IdAcquirenteDefault = GetAcquirenteDefault(prelievi),
                            IdCessionarioDefault = GetCessionarioDefault(prelievi),
                            IdDestinatarioDefault = GetDestinatarioDefault(prelievi),
                            Temperatura_Min = GetPercentile(temperature, 5),
                            Temperatura_Max = GetPercentile(temperature, 95),
                            Quantita_Min = GetPercentile(quantita, 5),
                            Quantita_Max = GetPercentile(quantita, 95)
                        });
                    }
                }

                db.Acquirenti = this.mapper.Map<List<AcquirenteDto>>(this.acquirentiRepository.Query.Where(a => a.Abilitato).ToList());
                db.Destinatari = this.mapper.Map<List<DestinatarioDto>>(this.destinatariRepository.Query.Where(a => a.Abilitato).ToList());
                db.Cessionari = this.mapper.Map<List<CessionarioDto>>(this.cessionariRepository.Query.Where(a => a.Abilitato).ToList());
            }

            dispositivo.DataUltimoDownload = DateTime.UtcNow;
            this.dispositiviRepository.Update(dispositivo);
            this.uow.SaveChanges();

            PushNotificationsService.Instance.Push(id);

            return db;
        }

        /// <summary>
        /// Caricamento dati prelievi latte
        /// </summary>
        /// <param name="dow"></param>
        public void Upload(UploadDto uploadDto)
        {
            var dispositivo = this.dispositiviRepository.GetById(uploadDto.IMEI);

            if (dispositivo != null && dispositivo.Attivo)
            {
                foreach (var prelievoDto in uploadDto.Prelievi)
                {
                    var prelievo = this.mapper.Map<PrelievoLatte>(prelievoDto);

                    prelievo.DeviceId = uploadDto.IMEI;

                    if(prelievo.IdAllevamento.HasValue)
                    {
                        var allevamento = this.allevamentiRepository.GetById(prelievo.IdAllevamento.Value);
                        if(allevamento != null)
                        {
                            var allevatore = this.utentiRepository.GetById(allevamento.IdUtente.Value);

                            prelievo.IdTipoLatte = allevatore != null ? allevatore.IdTipoLatte : (int?)null;
                        }
                    }

                    this.prelieviRepository.Add(prelievo);
                }

                dispositivo.Latitudine = GetDecimal(uploadDto.Lat);
                dispositivo.Longitudine = GetDecimal(uploadDto.Lng);
                dispositivo.DataUltimoUpload = DateTime.UtcNow;
                dispositivo.VersioneApp = uploadDto.VersioneApp;
                dispositivo.VersioneOS = uploadDto.VersioneOS;
                dispositivo.Marca = uploadDto.Marca;
                dispositivo.Modello = uploadDto.Modello;

                this.dispositiviRepository.Update(dispositivo);
                this.uow.SaveChanges();

                PushNotificationsService.Instance.Push(dispositivo.Id);
            }
        }

        #region Private Methods

        /// <summary>
        /// Recupero autocisterna per dispositivo
        /// </summary>
        /// <param name="dispositivo"></param>
        /// <returns></returns>
        private Autocisterna GetAutocisterna(DispositivoMobile dispositivo)
        {
            if (dispositivo.IdAutocisterna.HasValue)
                return this.autocisterneRepository.DbSet.FirstOrDefault(a => a.Id == dispositivo.IdAutocisterna.Value);
            else
                return this.autocisterneRepository.DbSet.FirstOrDefault(a => a.IdTrasportatore == dispositivo.IdTrasportatore.Value);
        }

        /// <summary>
        /// Recupero autocisterne per dispositivo
        /// </summary>
        /// <param name="dispositivo"></param>
        /// <returns></returns>
        private List<Autocisterna> GetAutocisterne(DispositivoMobile dispositivo)
        {
            return this.autocisterneRepository.DbSet
                //.Where(a => a.IdTrasportatore == dispositivo.IdTrasportatore.Value)
                .ToList();
        }

        /// <summary>
        /// Recupera l'acquirente più frequente
        /// </summary>
        /// <param name="prelievi"></param>
        /// <returns></returns>
        private int? GetAcquirenteDefault(List<PrelievoLatte> prelievi)
        {
            var idAcquirenteDefault = prelievi
                .Where(p => p.IdAcquirente.HasValue)
                .GroupBy(p => p.IdAcquirente)
                .OrderByDescending(gp => gp.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            if(idAcquirenteDefault.HasValue)
            {
                var acquirente = this.acquirentiRepository.GetById(idAcquirenteDefault.Value);
                idAcquirenteDefault = acquirente != null && acquirente.Abilitato ? idAcquirenteDefault : (int?)null;
            }

            return idAcquirenteDefault;
        }

        /// <summary>
        /// Recupera il cessionario più frequente
        /// </summary>
        /// <param name="prelievi"></param>
        /// <returns></returns>
        private int? GetCessionarioDefault(List<PrelievoLatte> prelievi)
        {
            var idCessionarioDeafult = prelievi
                .Where(p => p.IdCessionario.HasValue)
                .GroupBy(p => p.IdCessionario)
                .OrderByDescending(gp => gp.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            if (idCessionarioDeafult.HasValue)
            {
                var cessionario = this.cessionariRepository.GetById(idCessionarioDeafult.Value);
                idCessionarioDeafult = cessionario != null && cessionario.Abilitato ? idCessionarioDeafult : (int?)null;
            }

            return idCessionarioDeafult;
        }

        /// <summary>
        /// Recupera il destinatario più frequente
        /// </summary>
        /// <param name="prelievi"></param>
        /// <returns></returns>
        private int? GetDestinatarioDefault(List<PrelievoLatte> prelievi)
        {
            var idDestinatarioDefault = prelievi
                .Where(p => p.IdDestinatario.HasValue)
                .GroupBy(p => p.IdDestinatario)
                .OrderByDescending(gp => gp.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            if (idDestinatarioDefault.HasValue)
            {
                var destinatario = this.destinatariRepository.GetById(idDestinatarioDefault.Value);
                idDestinatarioDefault = destinatario != null && destinatario.Abilitato ? idDestinatarioDefault : (int?)null;
            }

            return idDestinatarioDefault;

        }

        /// <summary>
        /// Recupero prelievi
        /// </summary>
        /// <param name="idAllevamento"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private List<PrelievoLatte> GetPrelievi(int idAllevamento, int size)
        {
            return this.prelieviRepository
                .Query
                .Where(p => p.IdAllevamento == idAllevamento)
                .OrderByDescending(p => p.DataConsegna)
                .ToList();
        }

        /// <summary>
        /// Recupero prelievi
        /// </summary>
        /// <param name="idAllevamento"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private List<PrelievoLatte> GetPrelieviAutocisterna(int idAutocisterna, int size)
        {
            return this.prelieviRepository
                .Query
                .Where(p => p.IdAutocisterna == idAutocisterna)
                .ToList();
        }

        /// <summary>
        /// Calcolo percentile
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="excelPercentile"></param>
        /// <returns></returns>
        public decimal GetPercentile(decimal[] sequence, decimal excelPercentile)
        {
            if (sequence.Length == 0)
                return 0;

            if (excelPercentile > 100)
                excelPercentile = 100;

            Array.Sort(sequence);
            int N = sequence.Length;
            decimal n = (N - 1) * (excelPercentile / 100) + 1;
            // Another method: double n = (N + 1) * excelPercentile;
            if (n == 1) return sequence[0];
            else if (n == N) return sequence[N - 1];
            else
            {
                int k = (int)n;
                decimal d = n - k;
                return sequence[k - 1] + d * (sequence[k] - sequence[k - 1]);
            }
        }

        private static decimal? GetDecimal(decimal? dto)
        {
            return dto.HasValue && dto.Value != 0 ? dto.Value : (decimal?)null;
        }

        #endregion

        #endregion

    }
}
