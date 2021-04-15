using System;
using System.Web.Http;
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
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using WeCode.MVC.Attributes;
using WeCode.Application.Exceptions;
using RB.Excel;
using log4net;

namespace LatteMarche.WebApi.Controllers_Api
{

    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class PrelieviLatteController : ApiController
    {

        #region Fields

        private static ILog log = LogManager.GetLogger(typeof(PrelieviLatteController));

        private IPrelieviLatteService prelieviLatteService;
        private ISynchService synchService;
        private ISitraService sitraService;
        private ILottiService lottiService;
        private ILogsService logsService;
        private IUtentiService utentiService;

        private IMapper mapper;


        private bool PullEnabled { get { return Convert.ToBoolean(ConfigurationManager.AppSettings["synch_pull_enabled"]); } }
        private bool PushEnabled { get { return Convert.ToBoolean(ConfigurationManager.AppSettings["synch_push_enabled"]); } }
        private bool SitraEnabled { get { return Convert.ToBoolean(ConfigurationManager.AppSettings["send_sitra_enabled"]); } }

        #endregion

        #region Constructors

        public PrelieviLatteController(
            IMapper mapper, 
            IPrelieviLatteService prelieviLatteService, 
            ISynchService synchService, 
            ISitraService sitraService, 
            ILottiService lottiService, 
            ILogsService logsService, 
            IUtentiService utentiService
            )
        {
            this.mapper = mapper;

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

            return Ok(this.prelieviLatteService.Index());

        }

        [ViewItem(nameof(Details), "Prelievi latte", "Dettaglio")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Details(int id)
        {
            var dto = this.prelieviLatteService.Details(id);
            return Ok(dto);
        }

        [ViewItem(nameof(Save), "Prelievi latte", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] PrelievoLatteDto model)
        {
            try
            {
                this.prelieviLatteService.Validazione(model);

                if (model.Id == 0)
                    this.prelieviLatteService.Create(model);
                else
                    this.prelieviLatteService.Update(model);

                return Ok(model);
            }
            catch (ValidationException validationExc)
            {
                return BadRequest(validationExc.ModelStateDictionary);
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
                DateTime data = String.IsNullOrEmpty(day) ? DateTime.Today.AddDays(-1) : DateHelper.ConvertToDateTime(day).Value;

                this.LogDebug("InvioSitra", $"data [{data}]");

                // prelievi giorno precedente
                var prelieviDaInviare = this.prelieviLatteService.Sitra(data);
                //List<PrelievoLatteDto> prelieviDaInviare = this.mapper.Map<List<PrelievoLatteDto>>(prelievi);

                this.LogDebug("InvioSitra", $"prelievi giornalieri [{prelieviDaInviare.Count}]");

                // invio singoli prelievi
                response.PrelieviInviati = this.sitraService.InvioPrelievi(this.mapper.Map<List<PrelievoLatteDto>>(prelieviDaInviare));

                this.LogDebug("InvioSitra", $"prelievi inviati [{JsonConvert.SerializeObject(response.PrelieviInviati)}]");

                // fix valorizzazione codice sitra
                foreach (var prelievo in prelieviDaInviare)
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
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult Synch([FromUri] string day = "")
        {
            try
            {
                log.Info("Prelievi Synch executing");

                // scarica i dati dal cloud verso server locale
                this.LogDebug("Synch", $"PullEnabled [{this.PullEnabled}]");
                if (this.PullEnabled)
                    this.synchService.Pull();

                // carica i dati locali verso il cloud
                this.LogDebug("Synch", $"PushEnabled [{this.PushEnabled}]");
                if (this.PushEnabled)
                    this.synchService.Push();

                if (this.SitraEnabled)
                    InvioSitra(day);

                log.Info("Prelievi Synch executed");

                return Ok();
            }
            catch(Exception exc)
            {
                log.Error("Prelievi Synch", exc);
                return InternalServerError(exc);
            }
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
        public IHttpActionResult Search([FromUri] PrelieviLatteSearchDto searchDto)
        {
            try
            {
                UtenteDto utente = this.utentiService.GetByUsername(User.Identity.Name);
                var list = this.prelieviLatteService.Search(searchDto, utente.Id);
                return Ok(list);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [ViewItem(nameof(ExcelAllevatori), "Prelievi latte", "Excel Allevatori")]
        [HttpGet]
        public IHttpActionResult ExcelAllevatori([FromUri] PrelieviLatteSearchDto searchDto)
        {

            UtenteDto utente = this.utentiService.GetByUsername(User.Identity.Name);
            var list = this.prelieviLatteService.Search(searchDto, utente.Id);

            byte[] content = LatteMarche.WebApi.Helpers.ExcelAllevatoriHelper.MakeExcelTot(list);


            return File(content, "prelievi.xls", "application/vnd.ms-excel");

        }

        [ViewItem(nameof(ExcelTrasportatori), "Prelievi latte", "Excel Trasportatori")]
        [HttpGet]
        public IHttpActionResult ExcelTrasportatori([FromUri] PrelieviLatteSearchDto searchDto)
        {

            UtenteDto utente = this.utentiService.GetByUsername(User.Identity.Name);
            var list = this.prelieviLatteService.Search(searchDto, utente.Id);

            var records = this.mapper.Map<List<ExcelTrasportatoriViewModel>>(list);

            ExcelMaker helper = new ExcelMaker();

            byte[] content = helper.Make(records);


            return File(content, "prelievi.xlsx", "application/vnd.ms-excel");

        }


        [ViewItem(nameof(ExcelGiornalieri), "Prelievi latte", "Excel Giornalieri")]
        [HttpGet]
        public IHttpActionResult ExcelGiornalieri([FromUri] PrelieviLatteSearchDto searchDto)
        {

            UtenteDto utente = this.utentiService.GetByUsername(User.Identity.Name);
            var list = this.prelieviLatteService.Search(searchDto, utente.Id);

            byte[] content = LatteMarche.WebApi.Helpers.ExcelGiornalieriHelper.MakeExcel(list);


            return File(content, "prelievi.xls", "application/vnd.ms-excel");

        }

        [ViewItem(nameof(ExcelCooperative), "Prelievi latte", "Excel Cooperative")]
        [HttpGet]
        public IHttpActionResult ExcelCooperative([FromUri] PrelieviLatteSearchDto searchDto)
        {
            try
            {
                var utente = this.utentiService.GetByUsername(User.Identity.Name);
                var list = this.prelieviLatteService.Search(searchDto, utente.Id);

                var records = list
                    .Where(p => !String.IsNullOrEmpty(p.LottoConsegna))
                    .GroupBy(p => new { p.LottoConsegna })
                    .SelectMany(l => l.Select(i => new ExcelCooperativeViewModel()
                    {
                        LottoConsegna = i.LottoConsegna,
                        DataConsegna = l.Min(p => p.DataConsegna),
                        Acquirente = l.FirstOrDefault() != null ? l.First().Acquirente : "",
                        Trasportatore = l.FirstOrDefault() != null ? l.First().Trasportatore : "",
                        Quantita_Kg = l.Sum(p => p.Quantita),
                        Quantita_Lt = l.Sum(p => p.QuantitaLitri)
                    }))
                    .ToList<ExcelCooperativeViewModel>();

                records = records.Distinct().OrderBy(r => r.LottoConsegna).ToList();

                ExcelMaker helper = new ExcelMaker();

                byte[] content = helper.Make(records);

                return File(content, "lotti.xlsx", "application/vnd.ms-excel");
            }
            catch(Exception exc)
            {
                return InternalServerError(exc);
            }
        }


        protected IHttpActionResult File(byte[] content, string filename, string mediaType)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(content)
            };
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = filename
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            var response = ResponseMessage(result);

            return response;
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


        #endregion


    }
}