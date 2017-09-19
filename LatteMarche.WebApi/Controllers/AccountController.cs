using LatteMarche.WebApi.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace LatteMarche.WebApi.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {
        }

        public ActionResult Login()
        {
            return View();
        }

    }
}
