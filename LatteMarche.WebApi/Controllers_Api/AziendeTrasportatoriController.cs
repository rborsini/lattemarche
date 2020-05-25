using System;
using System.Web.Http;
using LatteMarche.Application.AziendeTrasportatori.Dtos;
using LatteMarche.Application.AziendeTrasportatori.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class AziendeTrasportatoriController : ApiController
    {

        #region Fields

        private IAziendeTrasportatoriService trasportatoriService;
        private IUtentiService utentiService;

        #endregion

        #region Constructors

        public AziendeTrasportatoriController(IAziendeTrasportatoriService trasportatoriService, IUtentiService utentiService)
		{
            this.trasportatoriService = trasportatoriService;
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Trasportatori", "Lista")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            try
            {
                return Ok(this.trasportatoriService.Index());
            }
            catch(Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Details), "Trasportatori", "Dettaglio")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.trasportatoriService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Dropdown), "Trasportatori", "Dropdown")]
        [HttpGet]
        public IHttpActionResult Dropdown()
        {
            try
            {
                var utente = this.utentiService.Details(User.Identity.Name);

                if (utente != null)
                    return Ok(this.trasportatoriService.DropDown());
                else
                    return Ok();
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }


        [ViewItem(nameof(Save), "Trasportatori", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] AziendaTrasportatoriDto model)
        {
            try
            {
                if (model.Id == 0)
                    return Ok(this.trasportatoriService.Create(model));
                else
                    return Ok(this.trasportatoriService.Update(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Delete), "Trasportatori", "Rimozione")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                this.trasportatoriService.Delete(id);
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
