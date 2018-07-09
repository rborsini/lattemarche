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
    public class PrelieviController: Controller
    {
        [ViewItem(nameof(Index), "Prelievi", "Lista")]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Index()
        {           
            return View();
        }

        [ViewItem(nameof(Edit), "Prelievi", "Modifica")]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Edit(long id)
        {
            return View();
        }

    }

}
