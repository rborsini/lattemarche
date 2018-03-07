using LatteMarche.WebApi.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace LatteMarche.WebApi.Controllers
{
    [MvcCustomAuthorize]
    public class TipiLatteController : Controller
    {
        // GET: TipiLatte
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Utente/New
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult New()
        {
            return View();
        }

        //
        // GET: /Utente/Details
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Details()
        {
            return View();
        }

    }
}