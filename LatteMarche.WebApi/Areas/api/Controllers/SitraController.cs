using AutoMapper;
using LatteMarche.Application.Lotti.Interfaces;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Application.Sitra.Interfaces;
using LatteMarche.Core.Models;
using LatteMarche.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    public class SitraController : ApiController
    {

        #region Fields

        private IPrelieviLatteService prelieviLatteService;
        private ISitraService sitraService;
        private ILottiService lottiService;

        #endregion

        #region Constructors

        public SitraController(IPrelieviLatteService prelieviLatteService, ISitraService sitraService, ILottiService lottiService)
        {
            this.prelieviLatteService = prelieviLatteService;
            this.sitraService = sitraService;
            this.lottiService = lottiService;
        }

        #endregion

        #region Methods

        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Send([FromUri] DateTime day)
        {
            try
            {
                SitraResponseVieModel response = new SitraResponseVieModel();

                // prelievi giorno precedente
                List<PrelievoLatte> prelievi = Mapper.Map<List<PrelievoLatte>>(this.prelieviLatteService.Search(new PrelieviLatteSearchDto()
                {
                    DataPeriodoInizio = day,
                    DataPeriodoFine = day,
                    InviatoSitra = false
                }).ToList());

                // invio singoli prelievi
                response.PrelieviInviati = this.sitraService.InvioPrelievi(Mapper.Map<List<PrelievoLatteDto>>(prelievi));

                // estrazione lotti dai nuovi prelievi
                var lotti = this.lottiService.GetLotti(prelievi);

                // invio lotti
                var lottiAggiornati = this.sitraService.InvioLotti(lotti);

                //persistenza database dei lotti inviati
                foreach (var lotto in lottiAggiornati)
                {
                    response.LottiInviati.Add(this.lottiService.Create(lotto));
                }

                return Ok(response);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        #endregion

    }
}