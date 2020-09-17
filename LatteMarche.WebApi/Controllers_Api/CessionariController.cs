using LatteMarche.Application.Cessionari.Dtos;
using LatteMarche.Application.Cessionari.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WeCode.MVC.Attributes;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class CessionariController : ApiController
    {

        #region Fields

        private ICessionariService cessionariService;
        private IUtentiService utentiService;

        #endregion

        #region Constructors

        public CessionariController(ICessionariService cessionariService, IUtentiService utentiService)
        {
            this.cessionariService = cessionariService;
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Cessionari", "Lista")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Index()
        {

            var acquirenti = this.cessionariService.Index();
            return Ok(acquirenti);

        }

        [ViewItem(nameof(Details), "Cessionari", "Dettaglio")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Details(int id)
        {

            return Ok(this.cessionariService.Details(id));

        }

        [ViewItem(nameof(Dropdown), "Cessionari", "Dropdown")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Dropdown()
        {

            var utente = this.utentiService.Details(User.Identity.Name);

            if (utente != null)
                return Ok(this.cessionariService.DropDown(utente.Id));
            else
                return Ok();

        }

        [ViewItem(nameof(Save), "Cessionari", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] CessionarioDto model)
        {

            if (model.Id == 0)
                return Ok(this.cessionariService.Create(model));
            else
                return Ok(this.cessionariService.Update(model));

        }

        [ViewItem(nameof(Delete), "Cessionari", "Rimozione")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {

            this.cessionariService.Delete(id);
            return Ok();

        }

        #endregion


    }
}