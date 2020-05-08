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
using WeCode.Data.Interfaces;

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
        private IRepository<PrelievoLatte, int> prelieviRepository;

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
            this.prelieviRepository = this.uow.Get<PrelievoLatte, int>();

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
                dispositivo = Mapper.Map<DispositivoMobile>(deviceInfo);
                dispositivo.DataRegistrazione = DateTime.Now;

                this.dispositiviRepository.Add(dispositivo);
                this.uow.SaveChanges();
            }

            return Mapper.Map<DispositivoDto>(this.dispositiviRepository.GetById(deviceInfo.Id));
        }

        /// <summary>
        /// Scaricamento dati di anagrafica e lookup
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
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

                            var prelievi = GetPrelievi(allevamento.Id, 100);
                            var temperature = prelievi.Where(p => p.Temperatura.HasValue).Select(p => p.Temperatura.Value).ToArray();
                            var quantita = prelievi.Where(p => p.Quantita.HasValue).Select(p => p.Quantita.Value).ToArray();

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
                                RagioneSociale = utente != null ? utente.RagioneSociale.Trim() : "",
                                Latitudine = allevamento?.Latitudine,
                                Longitudine = allevamento?.Longitudine,
                                IdAcquirenteDefault = GetAcquirenteDefault(prelievi),
                                IdDestinatarioDefault = GetDestinatarioDefault(prelievi),
                                Temperatura_Min = GetPercentile(temperature, 5),
                                Temperatura_Max = GetPercentile(temperature, 95),
                                Quantita_Min = GetPercentile(quantita, 5),
                                Quantita_Max = GetPercentile(quantita, 95)
                            });
                        }
                    }

                    db.TipiLatte = Mapper.Map<List<TipoLatteDto>>(this.tipiLatteRepository.Query);
                    db.Acquirenti = Mapper.Map<List<AcquirenteDto>>(this.acquirentiRepository.Query.ToList());
                    db.Destinatari = Mapper.Map<List<DestinatarioDto>>(this.destinataryRepository.Query.ToList());

                }

                dispositivo.DataUltimoDownload = DateTime.Now;
                this.dispositiviRepository.Update(dispositivo);
                this.uow.SaveChanges();
            }

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
                    var prelievo = Mapper.Map<PrelievoLatte>(prelievoDto);
                    this.prelieviRepository.Add(prelievo);
                }

                dispositivo.Latitudine = uploadDto.Lat;
                dispositivo.Longitudine = uploadDto.Lng;
                dispositivo.DataUltimoUpload = DateTime.Now;
                dispositivo.VersioneApp = uploadDto.VersioneApp;
                dispositivo.VersioneOS = uploadDto.VersioneOS;
                dispositivo.Marca = uploadDto.Marca;
                dispositivo.Modello = uploadDto.Modello;
                dispositivo.Nome = uploadDto.Nome;

                this.dispositiviRepository.Update(dispositivo);
                this.uow.SaveChanges();
            }
        }

        /// <summary>
        /// Recupera l'acquirente più frequente
        /// </summary>
        /// <param name="prelievi"></param>
        /// <returns></returns>
        private int? GetAcquirenteDefault(List<PrelievoLatte> prelievi)
        {
            return prelievi
                .Where(p => p.IdAcquirente.HasValue)
                .GroupBy(p => p.IdAcquirente)
                .OrderByDescending(gp => gp.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

        }

        /// <summary>
        /// Recupera il destinatario più frequente
        /// </summary>
        /// <param name="prelievi"></param>
        /// <returns></returns>
        private int? GetDestinatarioDefault(List<PrelievoLatte> prelievi)
        {
            return prelievi
                .Where(p => p.IdDestinatario.HasValue)
                .GroupBy(p => p.IdDestinatario)
                .OrderByDescending(gp => gp.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
        }

        private List<PrelievoLatte> GetPrelievi(int idAllevamento, int size)
        {
            return this.prelieviRepository
                .Query
                .Where(p => p.IdAllevamento == idAllevamento)
                .OrderByDescending(p => p.DataConsegna)
                .ToList();
        }

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

        #endregion

    }
}
