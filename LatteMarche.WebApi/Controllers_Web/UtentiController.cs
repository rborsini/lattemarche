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
    public class UtentiController : Controller
    {

        // GET: Utenti
        [ViewItem(nameof(Index), "Utenti - Elenco", "Lista")]
        public ActionResult Index()
        {
            return View();
        }

        [ViewItem(nameof(Edit), "Utenti - Dettaglio", "Sola lettura")]
        public ActionResult Edit()
        {
            return View();
        }

    }
}
