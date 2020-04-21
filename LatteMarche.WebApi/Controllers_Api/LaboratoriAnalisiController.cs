using System;
using System.Web.Http;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.AnalisiLatte.Interfaces;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class LaboratoriAnalisiController: ApiController
    {

        #region Fields

        private ILaboratoriAnalisiService laboratoriAnalisiService;

        #endregion

        #region Constructors

        public LaboratoriAnalisiController(ILaboratoriAnalisiService laboratoriAnalisiService)
        {
            this.laboratoriAnalisiService = laboratoriAnalisiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Laboratori Analisi", "Lista")]
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public IHttpActionResult Index()
        {
            try
            {
                return Ok(this.laboratoriAnalisiService.Index());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Details), "Laboratori Analisi", "Dettaglio")]
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.laboratoriAnalisiService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        #endregion

    }
}
