using LatteMarche.WebApi.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LatteMarche.WebApi.Areas.web.Controllers
{
    public class UtentiController : System.Web.Mvc.Controller
    {
        public UtentiController()
        {
        }

        // GET: Utenti
        [ViewItem(nameof(Index), "Utenti", "Lista")]
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Utente/New
        [ViewItem(nameof(New), "Utenti", "Aggiungi")]
        public ActionResult New()
        {
            return View();
        }

        //
        // GET: /Utente/Details
        [ViewItem(nameof(Details), "Utenti", "Modifica")]
        public ActionResult Details()
        {
            return View();
        }

    }
}
