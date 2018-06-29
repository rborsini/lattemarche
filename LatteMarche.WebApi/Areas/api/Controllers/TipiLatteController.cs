using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using LatteMarche.Application.TipiLatte.Interfaces;
using LatteMarche.Application.TipiLatte.Dtos;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class TipiLatteController : ApiController
    {

        #region Fields

        private ITipiLatteService tipiLatteService;

        #endregion

        #region Constructors

        public TipiLatteController(ITipiLatteService tipiLatteService)
        {
            this.tipiLatteService = tipiLatteService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Tipi latte", "Lista")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            try
            {
                return Ok(this.tipiLatteService.Index());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Details), "Tipi latte", "Dettaglio")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.tipiLatteService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [ViewItem(nameof(Create), "Tipi latte", "Creazione")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] TipoLatteDto model)
        {
            try
            {
                return Ok(this.tipiLatteService.Create(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Update), "Tipi latte", "Aggiornamento")]
        [HttpPut]
        public IHttpActionResult Update([FromBody] TipoLatteDto model)
        {
            try
            {
                return Ok(this.tipiLatteService.Update(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        #endregion

    }
}
