using LatteMarche.WebApi.Attributes;
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
    public class UtentiController : Controller
    {

        public UtentiController()
        {
        }

        // GET: Utenti
        [ViewItem(nameof(Index), "Utenti - Elenco", "Lista")]
        [ViewItem("Aggiungi", "Utenti - Elenco", "Aggiungi")]
        [ViewItem("SolaLettura", "Utenti - Elenco", "Sola lettura")]        
        [ViewItem("Rimuovi", "Utenti - Elenco", "Rimuovi")]
        public ActionResult Index()
        {
            return View();
        }

        [ViewItem(nameof(Edit), "Utenti - Dettaglio", "Sola lettura")]
        [ViewItem("Modifica", "Utenti - Dettaglio", "Modifica")]
        [ViewItem("Rimuovi", "Utenti - Dettaglio", "Rimuovi")]
        public ActionResult Edit()
        {
            return View();
        }

    }
}
