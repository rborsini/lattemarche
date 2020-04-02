using AutoMapper;
using LatteMarche.Application.Acquirenti.Interfaces;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.Destinatari.Interfaces;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.Application.LaboratoriAnalisi.Interfaces;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Core.Models;
using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;
using LatteMarche.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class SynchController : ApiController
    {
        #region Fields

        private ILaboratoriAnalisiService laboratoriAnalisiService;
        private IGiriService giriService;
        private IAcquirentiService acquirentiService;
        private IDestinatariService destinatariService;
        private IAllevamentiService allevamentiService;
        private IPrelieviLatteService prelieviLatteService;

        #endregion

        #region Constructors

        public SynchController(ILaboratoriAnalisiService laboratoriAnalisiService, IGiriService giriService, IAcquirentiService acquirentiService, IDestinatariService destinatariService, IAllevamentiService allevamentiService, IPrelieviLatteService prelieviLatteService)
        {
            this.laboratoriAnalisiService = laboratoriAnalisiService;
            this.giriService = giriService;
            this.acquirentiService = acquirentiService;
            this.destinatariService = destinatariService;
            this.allevamentiService = allevamentiService;
            this.prelieviLatteService = prelieviLatteService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Pull), "Synch", "Pull")]
        [HttpGet]
        public IHttpActionResult Pull(int idTrasportatore)
        {
            try
            {
                PullViewModel model = new PullViewModel();

                model.LaboratorioAnalisi = this.laboratoriAnalisiService.Index();
                model.Giri = this.giriService.GetGiriTrasportatore(idTrasportatore);

                foreach(var giro in model.Giri)
                {
                    var dettaglioGiro = this.giriService.Details(giro.Id);
                    giro.Items = dettaglioGiro.Items
                        .Where(i => i.Priorita.HasValue)
                        .OrderBy(i => i.Priorita)
                        .ToList();
                }

                model.Acquirenti = this.acquirentiService.Index();
                model.Destinatari = this.destinatariService.Index();
                model.Allevamenti = this.allevamentiService.Search(new Application.Allevamenti.Dtos.AllevamentiSearchDto());

                return Ok(model);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Push), "Synch", "Push")]
        [HttpPost]
        public IHttpActionResult Push([FromBody] List<PrelievoLatteDto> prelievi)
        {
            try
            {
                List<PrelievoLatte> prelieviList = prelievi.Select(p => Mapper.Map<PrelievoLatte>(p)).ToList();   
                return Ok(this.prelieviLatteService.Push(prelieviList));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        #endregion

    }
}