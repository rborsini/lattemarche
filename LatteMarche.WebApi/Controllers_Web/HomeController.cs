using LatteMarche.Application.Auth.Dtos;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.WebApi.Models;
using log4net;
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.UI;

namespace LatteMarche.WebApi.Controllers_Web
{
    public class HomeController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(HomeController));

        private bool mvcAuthEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["mvcAuthEnabled"]);
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
            log.Info("Home index");
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

            gestione.Items.Add(MakeViewModel("Dispositivi mobili", "Index", "Dispositivi"));
            gestione.Items.Add(MakeViewModel("Prelievi latte", "Index", "Prelievi"));
            gestione.Items.Add(MakeViewModel("Giri", "Index", "Trasportatori"));
            gestione.Items.Add(MakeViewModel("Utenti", "Index", "Utenti"));
            
            model.Add(gestione);

            MenuItemViewModel analisi = new MenuItemViewModel("Analisi");

            analisi.Items.Add(MakeViewModel("Analisi latte", "Index", "AnalisiLatte"));

            model.Add(analisi);

            MenuItemViewModel anagrafiche = new MenuItemViewModel("Anagrafiche");

            anagrafiche.Items.Add(MakeViewModel("Acquirenti", "Index", "Acquirenti"));
            anagrafiche.Items.Add(MakeViewModel("Cessionari", "Index", "Cessionari"));
            anagrafiche.Items.Add(MakeViewModel("Autocisterne", "Index", "Autocisterne"));
            anagrafiche.Items.Add(MakeViewModel("Destinatari", "Index", "Destinatari"));
            anagrafiche.Items.Add(MakeViewModel("Laboratori", "Index", "LaboratorioAnalisi"));
            anagrafiche.Items.Add(MakeViewModel("Tipi latte", "Index", "TipiLatte"));
            anagrafiche.Items.Add(MakeViewModel("Trasportatori", "Index", "AziendeTrasportatori"));

            model.Add(anagrafiche);

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
            return mvcAuthEnabled ? autorizzazioniService.Authorize(Session, username, "MVC", controller, action) : true;
        }
    }
}
