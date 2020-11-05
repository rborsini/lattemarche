using LatteMarche.Application.Auth.Dtos;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.WebApi.Filters;
using LatteMarche.WebApi.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace LatteMarche.WebApi.Controllers_Web
{
    [MvcActionFilter]
    [MvcExceptionFilter]
    public class AccountController : Controller
    {

        #region Fields

        private IUtentiService utentiService;

        #endregion

        #region Constructors

        public AccountController(IUtentiService utentiService)
        {
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        [AllowAnonymous]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [System.Web.Mvc.HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid && Membership.ValidateUser(model.Username, model.Password))
            {

                UtenteDto utenteDto = this.utentiService.Details(model.Username);

                if(utenteDto != null)
                {                    
                    Session["fullname"] = String.Format("{0} {1}", utenteDto.Nome, utenteDto.Cognome);
                }

                string tokenUrl = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "/Token");

                try
                {

                    tokenUrl = tokenUrl.Replace("https://", "http://");
                    string token = GetToken(tokenUrl, model.Username, model.Password);

                    if (!String.IsNullOrEmpty(token))
                    {
                        Session["token"] = token;
                        this.utentiService.SetToken(model.Username, token);
                    }

                    FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch 
                {
                    ViewBag.ErrorMessage = "Username o password errati";
                }

            }
            else
            {
                //LoggerConfig.MvcLog.WarnFormat("Login errato ({0})", model.Username);
                ViewBag.ErrorMessage = "Username o password errati";
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            string[] myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies[cookie].Domain = "";
            }

            Session["token"] = "";

            return RedirectToAction("Index", "Home");
        }

        private string GetToken(string url, string username, string password)
        {
            string token = "";

            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["grant_type"] = "password";
                values["username"] = username;
                values["password"] = password;

                var response = client.UploadValues(url, values);

                string responseString = Encoding.Default.GetString(response);

                dynamic responseObj = JsonConvert.DeserializeObject(responseString);
                token = responseObj.access_token;
            }

            return token;
        }


        #endregion

    }
}
