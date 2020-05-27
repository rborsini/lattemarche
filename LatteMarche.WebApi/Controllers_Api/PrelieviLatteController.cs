using System;
using System.Web.Http;
using LatteMarche.WebApi.Attributes;
using RB.Date;
using System.Collections.Generic;
using LatteMarche.Application.Synch.Interfaces;
using LatteMarche.Application.Sitra.Interfaces;
using System.Configuration;
using System.Linq;
using AutoMapper;
using LatteMarche.WebApi.Models;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.Logs.Interfaces;
using Newtonsoft.Json;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.Utenti.Dtos;

namespace LatteMarche.WebApi.Controllers_Api
{

    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class PrelieviLatteController : ApiController
    {

        #region Fields

        private IPrelieviLatteService prelieviLatteService;
        private ISynchService synchService;
        private ISitraService sitraService;
        private ILottiService lottiService;
        private ILogsService logsService;
        private IUtentiService utentiService;
        

        private bool PullEnabled { get { return Convert.ToBoolean(ConfigurationManager.AppSettings["synch_pull_enabled"]); } }
        private bool PushEnabled { get { return Convert.ToBoolean(ConfigurationManager.AppSettings["synch_push_enabled"]); } }
        private bool SitraEnabled { get { return Convert.ToBoolean(ConfigurationManager.AppSettings["send_sitra_enabled"]); } }

        #endregion

        #region Constructors

        public PrelieviLatteController(IPrelieviLatteService prelieviLatteService, ISynchService synchService, ISitraService sitraService, ILottiService lottiService, ILogsService logsService, IUtentiService utentiService)
        {
            this.prelieviLatteService = prelieviLatteService;
            this.synchService = synchService;
            this.sitraService = sitraService;
            this.lottiService = lottiService;
            this.logsService = logsService;
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Prelievi latte", "Lista")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            try
            {
                return Ok(this.prelieviLatteService.Index());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Details), "Prelievi latte", "Dettaglio")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.prelieviLatteService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Save), "Prelievi latte", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] PrelievoLatteDto model)
        {
            try
            {
                if (model.Id == 0)
                    this.prelieviLatteService.Create(model);
                else
                    this.prelieviLatteService.Update(model);


                return Ok(model);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Push), "Prelievi latte", "Push")]
        [HttpPost]
        public IHttpActionResult Push()
        {
            // carica i dati locali verso il cloud
            this.LogDebug("Push", $"PushEnabled [{this.PushEnabled}]");
            if (this.PushEnabled)
                this.synchService.Push();

            return Ok();
        }

        [ViewItem(nameof(Pull), "Prelievi latte", "Pull")]
        [HttpPost]
        public IHttpActionResult Pull()
        {
            // scarica i dati dal cloud verso server locale
            this.LogDebug("Pull", $"PullEnabled [{this.PullEnabled}]");
            if (this.PullEnabled)
                this.synchService.Pull();

            return Ok();
        }

        [ViewItem(nameof(Send), "Prelievi latte", "Invio sitra")]
        [HttpPost]
        public IHttpActionResult Send([FromUri] string day = "")
        {
            SitraResponseVieModel response = InvioSitra(day);
            return Ok(response);
        }

        private SitraResponseVieModel InvioSitra(string day)
        {
            SitraResponseVieModel response = new SitraResponseVieModel();

            response.SitraEnabled = this.SitraEnabled;

            // invio sitra
            this.LogDebug("InvioSitra", $"SitraEnabled [{this.SitraEnabled}]");
            if (this.SitraEnabled)
            {
                DateTime data = String.IsNullOrEmpty(day) ? DateTime.Today.AddDays(-1) : new DateHelper().ConvertToDateTime(day).Value;

                this.LogDebug("InvioSitra", $"data [{data}]");

                // prelievi giorno precedente
                List<PrelievoLatteDto> prelieviDaInviare = Mapper.Map<List<PrelievoLatteDto>>(this.prelieviLatteService.Search(new PrelieviLatteSearchDto()
                {
                    DataPeriodoInizio = data,
                    DataPeriodoFine = data
                }).ToList());

                this.LogDebug("InvioSitra", $"prelievi giornalieri [{prelieviDaInviare.Count}]");

                // invio singoli prelievi
                response.PrelieviInviati = this.sitraService.InvioPrelievi(Mapper.Map<List<PrelievoLatteDto>>(prelieviDaInviare));

                this.LogDebug("InvioSitra", $"prelievi inviati [{JsonConvert.SerializeObject(response.PrelieviInviati)}]");

                // fix valorizzazione codice sitra
                foreach(var prelievo in prelieviDaInviare)
                {
                    var prelievoInviato = response.PrelieviInviati.FirstOrDefault(p => p.Id == prelievo.Id);

                    if (prelievoInviato != null)
                        prelievo.CodiceSitra = prelievoInviato.CodiceSitra;
                }

                this.LogDebug("InvioSitra", $"prelieviDaInviare2 [{JsonConvert.SerializeObject(prelieviDaInviare)}]");

                // estrazione lotti dai nuovi prelievi
                var lotti = this.lottiService.GetLotti(prelieviDaInviare);

                this.LogDebug("InvioSitra", $"lotti [{JsonConvert.SerializeObject(lotti)}]");

                // invio lotti
                var lottiAggiornati = this.sitraService.InvioLotti(lotti);

                //this.LogDebug("InvioSitra", $"lottiAggiornati [{JsonConvert.SerializeObject(lottiAggiornati)}]");

                //persistenza database dei lotti inviati
                foreach (var lotto in lottiAggiornati)
                {
                    var lottoInviato = lotto;

                    if (lotto.Id == 0)
                        lottoInviato.Id = this.lottiService.Create(lotto).Id;

                    response.LottiInviati.Add(lottoInviato);
                }
            }

            //this.LogDebug("InvioSitra", $"response [{JsonConvert.SerializeObject(response)}]");
            return response;
        }

        [ViewItem(nameof(Synch), "Prelievi latte", "Sincronizzazione")]
        [HttpPost]
        public IHttpActionResult Synch([FromUri] string day = "")
        {
            // scarica i dati dal cloud verso server locale
            this.LogDebug("Synch", $"PullEnabled [{this.PullEnabled}]");
            if (this.PullEnabled)
                this.synchService.Pull();

            // carica i dati locali verso il cloud
            this.LogDebug("Synch", $"PushEnabled [{this.PushEnabled}]");
            if (this.PushEnabled)
                this.synchService.Push();

            SitraResponseVieModel response = InvioSitra(day);

            return Ok(response);
        }

        private void LogDebug(string method, string message)
        {
            this.logsService.Create(new Application.Logs.Dtos.LogRecordDto()
            {
                Identity = User.Identity.Name,
                Date = DateTime.Now,
                Level = "DEBUG",
                Logger = "api",
                Message = message,
                Thread = $"PrelieviLatteController.{method}"
            });
        }

        [ViewItem(nameof(Delete), "Prelievi latte", "Rimozione")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                this.prelieviLatteService.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [ViewItem(nameof(Search), "Prelievi latte", "Ricerca")]
        [HttpGet]
        public IHttpActionResult Search(string idAllevamento = "", string idTrasportatore = "", string idAcquirente = "", string idDestinatario = "", string idTipoLatte = "", string dal = "", string al = "")
        {

            UtenteDto utente = this.utentiService.GetByUsername(User.Identity.Name);

            switch (utente.IdProfilo)
            {
                case 3:     // profilo allevatore
                    idAllevamento = utente.Id.ToString();
                    break;
                case 5:     // profilo trasportatore
                    idTrasportatore = utente.Id.ToString();
                    break;
                //case 7:     // profilo acquirente
                //    var acquirente = this.acquirentiService.GetByIdUtente(utente.Id);
                //    idAcquirente = acquirente != null ? acquirente.Id.ToString() : "";
                //    break;
                //case 6:     // profilo destinatario
                //    var destinatario = this.destinatariService.GetByIdUtente(utente.Id);
                //    idDestinatario = destinatario != null ? destinatario.Id.ToString() : "";
                //    break;
            }

            //possibilità di mettere altri parametri come le date periodo prelievo
            try
            {
                return Ok(this.prelieviLatteService.Search(new PrelieviLatteSearchDto()
                {
                    idAllevamento = String.IsNullOrEmpty(idAllevamento) || idAllevamento == "undefined" ? (int?)null : Convert.ToInt32(idAllevamento),
                    idTrasportatore = String.IsNullOrEmpty(idTrasportatore) || idTrasportatore == "undefined" ? (int?)null : Convert.ToInt32(idTrasportatore),
                    idAcquirente = String.IsNullOrEmpty(idAcquirente) || idAcquirente == "undefined" ? (int?)null : Convert.ToInt32(idAcquirente),
                    idDestinatario = String.IsNullOrEmpty(idDestinatario) || idDestinatario == "undefined" ? (int?)null : Convert.ToInt32(idDestinatario),
                    idTipoLatte = String.IsNullOrEmpty(idTipoLatte) || idTipoLatte == "undefined" ? (int?)null : Convert.ToInt32(idTipoLatte),
                    DataPeriodoInizio = String.IsNullOrEmpty(dal) ? (DateTime?)null : new DateHelper().ConvertToDateTime(dal),
                    DataPeriodoFine = String.IsNullOrEmpty(al) ? (DateTime?)null : new DateHelper().ConvertToDateTime(al),
                }));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        #endregion


    }
}