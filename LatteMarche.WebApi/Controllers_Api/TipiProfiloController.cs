using System;
using System.Web.Http;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;
using WeCode.MVC.Attributes;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class TipiProfiloController : ApiController
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
        [ETag]
        public IHttpActionResult Index()
        {

            return Ok(this.tipiProfiloService.Index());

        }

        [ViewItem(nameof(Details), "Tipi profilo", "Dettaglio")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Details(int id)
        {

            return Ok(this.tipiProfiloService.Details(id));

        }

        [ViewItem(nameof(Dropdown), "Tipi profilo", "Dropdown")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Dropdown()
        {

            return Ok(this.tipiProfiloService.DropDown());

        }

        #endregion

    }
}
