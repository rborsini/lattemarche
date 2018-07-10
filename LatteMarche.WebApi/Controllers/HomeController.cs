using LatteMarche.WebApi.Models;
using LatteMarche.Application.Ruoli.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Application.Utenti.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.UI;

namespace LatteMarche.WebApi.Controllers
{
    public class HomeController : Controller
    {
        private IAutorizzazioniService autorizzazioniService;
        private IUtentiService utentiService;

        public HomeController (IAutorizzazioniService autorizzazioniService, IUtentiService utentiService)
        {
            this.autorizzazioniService = autorizzazioniService;
            this.utentiService = utentiService;
        }

        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Index()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";

            return View();
        }

        /// <summary>
        /// Default page for unauthorized request
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult Menu(string currentController)
        {
            ViewBag.currentController = currentController;

            MenuViewModel model = new MenuViewModel();

            MenuItemViewModel gestione = new MenuItemViewModel("Gestione");
            gestione.Items.Add(MakeViewModel("Utenti", "Index", "Utenti"));
            gestione.Items.Add(MakeViewModel("Trasportatori", "Index", "Trasportatori"));
            gestione.Items.Add(MakeViewModel("Prelievi latte", "Index", "Prelievi"));
            gestione.Items.Add(MakeViewModel("Tipi latte", "Index", "TipiLatte"));
            gestione.Items.Add(MakeViewModel("Acquirenti", "Index", "Acquirenti"));
            gestione.Items.Add(MakeViewModel("Destinatari", "Index", "Destinatari"));
            gestione.Items.Add(MakeViewModel("Laboratorio analisi", "Index", "LaboratorioAnalisi"));
            gestione.Items.Add(MakeViewModel("Sitra", "Index", "Sitra"));
            gestione.Items.Add(MakeViewModel("Normative", "Index", "Documenti"));
            model.Add(gestione);

            MenuItemViewModel amministrazione = new MenuItemViewModel("Amministrazione");
            amministrazione.Items.Add(MakeViewModel("Azioni", "Index", "Azioni"));
            amministrazione.Items.Add(MakeViewModel("Ruoli", "Index", "Ruoli"));
            model.Add(amministrazione);


            UtenteDto utente = this.utentiService.Details(User.Identity.Name);

            return PartialView("_Menu", model);
        }

        private MenuItemViewModel MakeViewModel(string title, string action, string controller)
        {
            return MakeViewModel(title, action, controller, "");
        }

        private MenuItemViewModel MakeViewModel(string title, string action, string controller, string area)
        {
            return new MenuItemViewModel(title, action, controller, area, User.Identity.Name, IsAuthorized(controller, action, User.Identity.Name));
        }

        private bool IsAuthorized(string controller, string action, string username)
        {
            return autorizzazioniService.Authorize(Session, username, "MVC", controller, action);
        }
    }
}
