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
    public class RuoliController : Controller
    {
        [ViewItem(nameof(Index), "Ruoli", "Lista")]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Index()
        {           
            return View();
        }

        [ViewItem(nameof(Edit), "Ruoli", "Modifica")]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Edit(long id)
        {
            return View();
        }

    }
}
