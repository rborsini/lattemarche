using System;
using System.Web.Http;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class TipiProfiloController: ApiController
    {

        #region Fields

        private ITipiProfiloService tipiProfiloService;

        #endregion

        #region Constructors

        public TipiProfiloController(ITipiProfiloService tipiProfiloService)
        {
            this.tipiProfiloService = tipiProfiloService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Tipi profilo", "Lista")]
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public IHttpActionResult Index()
        {
            try
            {
                return Ok(this.tipiProfiloService.Index());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Details), "Tipi profilo", "Dettaglio")]
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.tipiProfiloService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [ViewItem(nameof(Dropdown), "Tipi profilo", "Dropdown")]
        [HttpGet]
        public IHttpActionResult Dropdown()
        {
            try
            {
                return Ok(this.tipiProfiloService.DropDown());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        #endregion

    }
}
