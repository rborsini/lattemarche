using System;
using System.Web.Http;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class TipiLatteController : ApiController
    {

        #region Fields

        private ITipiLatteService tipiLatteService;
        private IUtentiService utentiService;

        #endregion

        #region Constructors

        public TipiLatteController(ITipiLatteService tipiLatteService, IUtentiService utentiService)
        {
            this.tipiLatteService = tipiLatteService;
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Tipi latte", "Lista")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            try
            {
                return Ok(this.tipiLatteService.Index());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Details), "Tipi latte", "Dettaglio")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.tipiLatteService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [ViewItem(nameof(Dropdown), "Tipi latte", "Dropdown")]
        [HttpGet]
        public IHttpActionResult Dropdown()
        {
            try
            {
                return Ok(this.tipiLatteService.DropDown());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [ViewItem(nameof(Save), "Tipi latte", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] TipoLatteDto model)
        {
            try
            {
                if(model.Id == 0)
                    return Ok(this.tipiLatteService.Create(model));
                else
                    return Ok(this.tipiLatteService.Update(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }


        [ViewItem(nameof(Delete), "Tipi latte", "Rimozione")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                this.tipiLatteService.Delete(id);
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
