
using LatteMarche.Application.Dashboard.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class WidgetsController : ApiController
    {

        #region Fields

        private IWidgetsService widgetsService;
        private IUtentiService utentiService;

        #endregion

        #region Constructor

        public WidgetsController(IWidgetsService widgetsService, IUtentiService utentiService)
        {
            this.widgetsService = widgetsService;
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Sommario), "Widgets", "Sommario")]
        [HttpGet]
        public IHttpActionResult Sommario()
        {
            try
            {
                var user = this.utentiService.Details(User.Identity.Name);
                return Ok(this.widgetsService.WidgetSommario(user.Id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [ViewItem(nameof(Acquirenti), "Widgets", "Acquirenti")]
        [HttpGet]
        public IHttpActionResult Acquirenti()
        {
            try
            {
                var user = this.utentiService.Details(User.Identity.Name);
                return Ok(this.widgetsService.WidgetAcquirenti(user.Id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [ViewItem(nameof(TipiLatte), "Widgets", "TipiLatte")]
        [HttpGet]
        public IHttpActionResult TipiLatte()
        {
            try
            {
                var user = this.utentiService.Details(User.Identity.Name);
                return Ok(this.widgetsService.WidgetTipiLatte(user.Id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        #endregion


    }
}