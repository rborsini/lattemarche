using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LatteMarche.WebApi.Areas.web.Controllers
{
    public class UtentiController : System.Web.Mvc.Controller
    {
        public UtentiController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
