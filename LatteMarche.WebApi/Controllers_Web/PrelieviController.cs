using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;
using RB.Date;
using System;
using System.Web.Mvc;

namespace LatteMarche.WebApi.Controllers_Web
{
    [MvcCustomAuthorize]
    [MvcActionFilter]
    [MvcExceptionFilter]
    public class PrelieviController: Controller
    {

        #region Fields

        private IUtentiService utentiService;

        #endregion

        #region Constructor

        public PrelieviController(IUtentiService utentiService)
        {
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Prelievi", "Lista")]
        public ActionResult Index()
        {
            var utente = this.utentiService.Details(User.Identity.Name);
            int idProfilo = utente != null ? utente.IdProfilo : 0;

            return View(idProfilo);
        }

        #endregion

    }

}
