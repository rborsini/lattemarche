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
    public class AutocisterneController : Controller
    {
        [ViewItem(nameof(Index), "Autocisterne", "Lista")]
        [ViewItem("Aggiungi", "Autocisterne", "Aggiungi")]
        [ViewItem("Modifica", "Autocisterne", "Modifica")]
        [ViewItem("Rimuovi", "Autocisterne", "Rimuovi")]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Index()
        {           
            return View();
        }

    }
}
