using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace LatteMarche.WebApi.Controllers_Web
{
    [MvcCustomAuthorize]
    [MvcActionFilter]
    [MvcExceptionFilter]
    public class AnalisiController : Controller
    {
        #region Fields

        private IUtentiService utentiService;

        #endregion

        #region Constructor

        public AnalisiController(IUtentiService utentiService)
        {
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Produttori), "Analisi", "Produttori")]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Produttori()
        {
            var utente = this.utentiService.Details(User.Identity.Name);
            int idProfilo = utente != null ? utente.IdProfilo : 0;

            return View(idProfilo);
        }

        [ViewItem(nameof(Map), "Analisi", "Map")]
        //[OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Map()
        {
            var utente = this.utentiService.Details(User.Identity.Name);
            int idProfilo = utente != null ? utente.IdProfilo : 0;

            return View(idProfilo);
        }

        #endregion

    }
}