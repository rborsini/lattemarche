
using LatteMarche.Application.Dashboard.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.WebApi.Filters;
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
        [ETag]
        public IHttpActionResult Sommario()
        {

            var user = this.utentiService.Details(User.Identity.Name);
            return Ok(this.widgetsService.WidgetSommario(user.Id));

        }

        [ViewItem(nameof(Acquirenti), "Widgets", "Acquirenti")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Acquirenti()
        {

            var user = this.utentiService.Details(User.Identity.Name);
            return Ok(this.widgetsService.WidgetAcquirenti(user.Id));

        }

        [ViewItem(nameof(TipiLatte), "Widgets", "TipiLatte")]
        [HttpGet]
        [ETag]
        public IHttpActionResult TipiLatte()
        {

            var user = this.utentiService.Details(User.Identity.Name);
            return Ok(this.widgetsService.WidgetTipiLatte(user.Id));

        }

        #endregion


    }
}