using System;
using System.Web.Http;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Application.PrelieviLatte.Dtos;
using Newtonsoft.Json.Linq;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;
using RB.Date;
using System.Collections.Generic;
using LatteMarche.Application.Synch.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using LatteMarche.Application.Sitra.Interfaces;
using LatteMarche.Application.Lotti.Interfaces;
using System.Configuration;
using System.Linq;
using AutoMapper;

namespace LatteMarche.WebApi.Areas.api.Controllers
{

    [ApiCustomAuthorize]
    public class PrelieviLatteController : ApiController
    {

        #region Fields

        private IPrelieviLatteService prelieviLatteService;
        private ISynchService synchService;
        private ISitraService sitraService;
        private ILottiService lottiService;

        private bool PullEnabled { get { return Convert.ToBoolean(ConfigurationManager.AppSettings["synch_pull_enabled"]); } }
        private bool PushEnabled { get { return Convert.ToBoolean(ConfigurationManager.AppSettings["synch_push_enabled"]); } }
        private bool SitraEnabled { get { return Convert.ToBoolean(ConfigurationManager.AppSettings["send_sitra_enabled"]); } }

        #endregion

        #region Constructors

        public PrelieviLatteController(IPrelieviLatteService prelieviLatteService, ISynchService synchService, ISitraService sitraService, ILottiService lottiService)
        {
            this.prelieviLatteService = prelieviLatteService;
            this.synchService = synchService;
            this.sitraService = sitraService;
            this.lottiService = lottiService;
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

        [ViewItem(nameof(Synch), "Prelievi latte", "Sincronizzazione")]
        [HttpPost]
        //[AllowAnonymous]
        public IHttpActionResult Synch()
        {
            // scarica i dati dal cloud verso server locale
            if(this.PullEnabled)
                this.synchService.Pull();

            // carica i dati locali verso il cloud
            if(this.PushEnabled)
                this.synchService.Push();

            // invio sitra
            if (this.SitraEnabled)
            {
                // prelievi giorno precedente
                List<PrelievoLatte> prelieviDaInviare = Mapper.Map<List<PrelievoLatte>>(this.prelieviLatteService.Search(new PrelieviLatteSearchDto()
                {
                    DataPeriodoInizio = DateTime.Today.AddDays(-1),
                    DataPeriodoFine = DateTime.Today.AddDays(-1),
                    InviatoSitra = false
                }).ToList());

                // invio singoli prelievi
                this.sitraService.InvioPrelievi(Mapper.Map<List<PrelievoLatteDto>>(prelieviDaInviare));

                // estrazione lotti dai nuovi prelievi
                var lotti = this.lottiService.GetLotti(prelieviDaInviare);

                // invio lotti
                var lottiAggiornati = this.sitraService.InvioLotti(lotti);

                //persistenza database dei lotti inviati
                foreach (var lotto in lottiAggiornati)
                {
                    this.lottiService.Create(lotto);
                }
            }

            return Ok("ok");
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
        //[CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public IHttpActionResult Search(string idAllevamento = "", string dal = "", string al = "")
        {
            //possibilità di mettere altri parametri come le date periodo prelievo
            try
            {
                return Ok(this.prelieviLatteService.Search(new PrelieviLatteSearchDto()
                {
                    idAllevamento = String.IsNullOrEmpty(idAllevamento) || idAllevamento == "undefined" ? (int?)null : Convert.ToInt32(idAllevamento),
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