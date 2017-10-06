using LatteMarche.WebApi.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.UI;

namespace LatteMarche.WebApi.Controllers
{
    [MvcCustomAuthorize]
    public class UtentiController : Controller
    {
        public UtentiController()
        {
        }

        // GET: Utenti
        [ViewItem(nameof(Index), "Utenti", "Lista")]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Utente/New
        [ViewItem(nameof(New), "Utenti", "Aggiungi")]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult New()
        {
            return View();
        }

        //
        // GET: /Utente/Details
        [ViewItem(nameof(Details), "Utenti", "Modifica")]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Details()
        {
            return View();
        }

    }
}
