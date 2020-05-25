using System;
using System.Web.Http;
using LatteMarche.WebApi.Attributes;
using LatteMarche.Application.Destinatari.Interfaces;
using LatteMarche.Application.Destinatari.Dtos;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.Utenti.Interfaces;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class DestinatariController: ApiController
    {

        #region Fields

        private IDestinatariService destinatariService;
        private IUtentiService utentiService;

        #endregion

        #region Constructors

        public DestinatariController(IDestinatariService destinatariService, IUtentiService utentiService)
        {
            this.destinatariService = destinatariService;
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Destinatari", "Lista")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            try
            {
                return Ok(this.destinatariService.Index());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Details), "Destinatari", "Dettaglio")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.destinatariService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [ViewItem(nameof(Dropdown), "Destinatari", "Dropdown")]
        [HttpGet]
        public IHttpActionResult Dropdown()
        {
            try
            {
                var utente = this.utentiService.Details(User.Identity.Name);

                if (utente != null)
                    return Ok(this.destinatariService.DropDown(utente.Id));
                else
                    return Ok();
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [ViewItem(nameof(Save), "Destinatari", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] DestinatarioDto model)
        {
            try
            {
                if(model.Id == 0)
                    return Ok(this.destinatariService.Create(model));
                else
                    return Ok(this.destinatariService.Update(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Delete), "Destinatari", "Rimozione")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                this.destinatariService.Delete(id);
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
