using LatteMarche.WebApi.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace LatteMarche.WebApi.Controllers
{
    //[MvcCustomAuthorize]
    public class TipiLatteController : Controller
    {
        [ViewItem(nameof(Index), "Tipi latte", "Lista")]
        public ActionResult Index()
        {
            return View();
        }

        
        [ViewItem(nameof(New), "Tipi latte", "Aggiungi")]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult New()
        {
            return View();
        }

        
        [ViewItem(nameof(Details), "Tipi latte", "Modifica")]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Details()
        {
            return View();
        }

    }
}