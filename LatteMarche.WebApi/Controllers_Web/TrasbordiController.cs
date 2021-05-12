using LatteMarche.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LatteMarche.WebApi.Controllers_Web
{
    [MvcCustomAuthorize]
    [MvcActionFilter]
    [MvcExceptionFilter]
    public class TrasbordiController : Controller
    {
        private const string PAGE_NAME = "Trasbordi";


        [ViewItem(nameof(Index), PAGE_NAME, "Lista")]
        public ActionResult Index()
        {
            return View();
        }

        [ViewItem(nameof(Edit), PAGE_NAME, "Sola lettura")]
        public ActionResult Edit()
        {
            return View();
        }
    }
}