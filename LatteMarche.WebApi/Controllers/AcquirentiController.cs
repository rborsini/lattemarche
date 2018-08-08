using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;
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
    [MvcActionFilter]
    [MvcExceptionFilter]
    public class AcquirentiController : Controller
    {
        [ViewItem(nameof(Index), "Acquirenti", "Lista")]
        [ViewItem("Aggiungi", "Acquirenti", "Aggiungi")]
        [ViewItem("Modifica", "Acquirenti", "Modifica")]
        [ViewItem("Rimuovi", "Acquirenti", "Rimuovi")]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Index()
        {           
            return View();
        }

    }
}
