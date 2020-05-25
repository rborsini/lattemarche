using LatteMarche.Application.Cessionari.Dtos;
using LatteMarche.Application.Cessionari.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

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
        public IHttpActionResult Index()
        {
            try
            {

                var acquirenti = this.cessionariService.Index();
                return Ok(acquirenti);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Details), "Cessionari", "Dettaglio")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.cessionariService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Dropdown), "Cessionari", "Dropdown")]
        [HttpGet]
        public IHttpActionResult Dropdown()
        {
            try
            {
                var utente = this.utentiService.Details(User.Identity.Name);

                if (utente != null)
                    return Ok(this.cessionariService.DropDown(utente.Id));
                else
                    return Ok();
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [ViewItem(nameof(Save), "Cessionari", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] CessionarioDto model)
        {
            try
            {
                if (model.Id == 0)
                    return Ok(this.cessionariService.Create(model));
                else
                    return Ok(this.cessionariService.Update(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Delete), "Cessionari", "Rimozione")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                this.cessionariService.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        #endregion


    }
}