
using LatteMarche.Application.Dashboard.Dtos;
using LatteMarche.Application.Dashboard.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.WebApi.Filters;
using RB.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WeCode.MVC.Attributes;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class WidgetsController : ApiController
    {

        #region Constants

        private const string PAGE_NAME = "Widgets";

        #endregion

        #region Fields

        private IWidgetsService widgetsService;
        private IUtentiService utentiService;
        private IAnalisiComparativaService analisiComparativaService;
        private IAnalisiQualitativaService analisiQualitativaService;
        private IAnalisiQuantitativaService analisiQuantitativaService;

        #endregion

        #region Constructor

        public WidgetsController(
            IWidgetsService widgetsService, 
            IUtentiService utentiService, 
            IAnalisiComparativaService analisiComparativaService,
            IAnalisiQualitativaService analisiQualitativaService,
            IAnalisiQuantitativaService analisiQuantitativaService)
        {
            this.widgetsService = widgetsService;
            this.utentiService = utentiService;
            this.analisiComparativaService = analisiComparativaService;
            this.analisiQualitativaService = analisiQualitativaService;
            this.analisiQuantitativaService = analisiQuantitativaService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Dashboard - Sommario
        /// </summary>
        /// <returns></returns>
        [ViewItem(nameof(Sommario), PAGE_NAME, "Sommario")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Sommario()
        {

            var user = this.utentiService.Details(User.Identity.Name);
            return Ok(this.widgetsService.WidgetSommario(user.Id));

        }

        /// <summary>
        /// Dashboard - Grafico acquirenti
        /// </summary>
        /// <returns></returns>
        [ViewItem(nameof(Acquirenti), PAGE_NAME, "Acquirenti")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Acquirenti()
        {

            var user = this.utentiService.Details(User.Identity.Name);
            return Ok(this.widgetsService.WidgetAcquirenti(user.Id));

        }

        /// <summary>
        /// Dashboard - Grafico tipo latte
        /// </summary>
        /// <returns></returns>
        [ViewItem(nameof(TipiLatte), PAGE_NAME, "TipiLatte")]
        [HttpGet]
        [ETag]
        public IHttpActionResult TipiLatte()
        {

            var user = this.utentiService.Details(User.Identity.Name);
            return Ok(this.widgetsService.WidgetTipiLatte(user.Id));

        }

        /// <summary>
        /// Analisi produttori - Analisi quantitativa
        /// </summary>
        /// <param name="idAllevamento"></param>
        /// <param name="da"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        [ViewItem(nameof(AnalisiQuantitativa), PAGE_NAME, "Analisi Quantitativa")]
        [HttpGet]
        [ETag]
        public IHttpActionResult AnalisiQuantitativa(int idAllevamento, string da, string a)
        {
            var dto = this.analisiQuantitativaService.Load(idAllevamento, DateHelper.ConvertToDateTime(da).Value, DateHelper.ConvertToDateTime(a).Value);
            return Ok(dto);
        }

        /// <summary>
        /// Analisi produttori - Analisi qualitativa
        /// </summary>
        /// <param name="idAllevamento"></param>
        /// <param name="da"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        [ViewItem(nameof(AnalisiQualitativa), PAGE_NAME, "Analisi Qualitativa")]
        [HttpGet]
        [ETag]
        public IHttpActionResult AnalisiQualitativa(int idAllevamento, string da, string a)
        {
            var dto = this.analisiQualitativaService.Load(idAllevamento, DateHelper.ConvertToDateTime(da).Value, DateHelper.ConvertToDateTime(a).Value);
            return Ok(dto);
        }

        /// <summary>
        /// Analisi produttori - Analisi comparativa
        /// </summary>
        /// <param name="idAllevamento"></param>
        /// <param name="da"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        [ViewItem(nameof(AnalisiComparativa), PAGE_NAME, "Analisi Comparativa")]
        [HttpGet]
        [ETag]
        public IHttpActionResult AnalisiComparativa(int idAllevamento, string da, string a)
        {
            var dto = this.analisiComparativaService.Load(idAllevamento, DateHelper.ConvertToDateTime(da).Value, DateHelper.ConvertToDateTime(a).Value);
            return Ok(dto);
        }

        #endregion


    }
}