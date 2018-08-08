using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace LatteMarche.WebApi.Controllers
{
    [MvcCustomAuthorize]
    [MvcActionFilter]
    [MvcExceptionFilter]
    public class TipiLatteController : Controller
    {
        [ViewItem(nameof(Index), "Tipi latte", "Lista")]
        [ViewItem("Aggiungi", "Tipi latte", "Aggiungi")]
        [ViewItem("Modifica", "Tipi latte", "Modifica")]
        [ViewItem("Rimuovi", "Tipi latte", "Rimuovi")]
        public ActionResult Index()
        {
            return View();
        }

    }
}