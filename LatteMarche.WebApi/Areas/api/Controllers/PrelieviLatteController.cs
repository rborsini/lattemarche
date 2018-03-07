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

        [HttpGet]
        [HttpPost]
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

        [HttpPost]
        //[AllowAnonymous]
        public IHttpActionResult Synch()
        {
            List<PrelievoLatte> nuoviPrelievi = new List<PrelievoLatte>();

            // scarica i dati dal cloud verso server locale
            if(this.PullEnabled)
                synchService.Pull();

            // carica i dati locali verso il cloud
            if(this.PushEnabled)
                nuoviPrelievi = synchService.Push();

            // invio sitra
            if(this.SitraEnabled)
            {
                // estrazione lotti dai nuovi prelievi
                var lotti = lottiService.GetLotti(nuoviPrelievi);

                // invio lotti Sitra
                var lottiAggiornati = sitraService.InvioLotti(lotti);

                // persistenza database dei lotti inviati
                foreach (var lotto in lottiAggiornati)
                {
                    lottiService.Create(lotto);
                }
            }

            return Ok("ok");
        }


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

        [HttpGet]
        [HttpPost]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
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