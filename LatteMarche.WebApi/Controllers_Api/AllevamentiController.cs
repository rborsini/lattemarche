using System;
using System.Web.Http;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Allevamenti;
using Newtonsoft.Json.Linq;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.Utenti.Interfaces;
using WeCode.MVC.Attributes;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class AllevamentiController : ApiController
    {

        #region Fields

        private IAllevamentiService allevamentiService;
        private IUtentiService utentiService;

        #endregion

        #region Constructors

        public AllevamentiController(IAllevamentiService allevamentiService, IUtentiService utentiService)
        {
            this.allevamentiService = allevamentiService;
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Dropdown), "Allevamenti", "Dropdown")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Dropdown()
        {
            var utente = this.utentiService.Details(User.Identity.Name);

            if (utente != null)
                return Ok(this.allevamentiService.DropDown(utente.Id));
            else
                return Ok();
        }

        #endregion


    }
}
