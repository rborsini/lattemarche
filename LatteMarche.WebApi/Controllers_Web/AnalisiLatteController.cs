using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.UI;

namespace LatteMarche.WebApi.Controllers_Web
{
    [MvcCustomAuthorize]
    [MvcActionFilter]
    [MvcExceptionFilter]
    public class AnalisiLatteController : Controller
    {
        #region Fields

        private IUtentiService utentiService;

        #endregion

        #region Constructor

        public AnalisiLatteController(IUtentiService utentiService)
        {
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Analisi latte", "Lista")]
        public ActionResult Index()
        {
            var utente = this.utentiService.Details(User.Identity.Name);
            int idProfilo = utente != null ? utente.IdProfilo : 0;

            return View(idProfilo);
        }

        #endregion

    }
}
