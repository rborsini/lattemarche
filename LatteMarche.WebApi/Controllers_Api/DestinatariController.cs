using System;
using System.Web.Http;
using LatteMarche.Application.Destinatari.Interfaces;
using LatteMarche.Application.Destinatari.Dtos;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.Utenti.Interfaces;
using WeCode.MVC.Attributes;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class DestinatariController : ApiController
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
        [ETag]
        public IHttpActionResult Index()
        {

            return Ok(this.destinatariService.Index());

        }

        [ViewItem(nameof(Details), "Destinatari", "Dettaglio")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Details(int id)
        {

            return Ok(this.destinatariService.Details(id));

        }

        [ViewItem(nameof(Dropdown), "Destinatari", "Dropdown")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Dropdown()
        {

            var utente = this.utentiService.Details(User.Identity.Name);

            if (utente != null)
                return Ok(this.destinatariService.DropDown(utente.Id));
            else
                return Ok();

        }

        [ViewItem(nameof(Save), "Destinatari", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] DestinatarioDto model)
        {

            if (model.Id == 0)
                return Ok(this.destinatariService.Create(model));
            else
                return Ok(this.destinatariService.Update(model));

        }

        [ViewItem(nameof(Delete), "Destinatari", "Rimozione")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {

            this.destinatariService.Delete(id);
            return Ok();

        }

        #endregion

    }
}
