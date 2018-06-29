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
    public class SitraController : Controller
    {
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Index()
        {
            return View();
        }

    }
}
