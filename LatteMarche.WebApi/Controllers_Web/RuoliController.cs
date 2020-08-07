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
    public class RuoliController : Controller
    {
        [ViewItem(nameof(Index), "Ruoli", "Lista")]
        public ActionResult Index()
        {           
            return View();
        }

        [ViewItem(nameof(Edit), "Ruoli", "Modifica")]
        public ActionResult Edit(long id)
        {
            return View();
        }

        [ViewItem(nameof(New), "Ruoli", "Aggiungi")]
        public ActionResult New()
        {
            return View();
        }

    }
}
