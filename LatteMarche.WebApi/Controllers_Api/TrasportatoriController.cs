using LatteMarche.Application.Trasportatori.Interfaces;
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
    public class TrasportatoriController : ApiController
    {
        #region Fields

        private ITrasportatoriService trasportatoriService;
        private IUtentiService utentiService;

        #endregion

        #region Constructor

        public TrasportatoriController(ITrasportatoriService trasportatoriService, IUtentiService utentiService)
        {
            this.trasportatoriService = trasportatoriService;
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Dropdown), "Trasportatori", "Dropdown")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Dropdown()
        {
            var utente = this.utentiService.Details(User.Identity.Name);
            var dropDown = this.trasportatoriService.DropDown(utente.Id);

            return Ok(dropDown);
        }

        #endregion

    }
}