using System;
using System.Web.Http;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.WebApi.Filters;
using WeCode.MVC.Attributes;

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
        [ETag]
        public IHttpActionResult Index()
        {

            return Ok(this.tipiLatteService.Index());

        }

        [ViewItem(nameof(Details), "Tipi latte", "Dettaglio")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Details(int id)
        {

            return Ok(this.tipiLatteService.Details(id));

        }

        [ViewItem(nameof(Dropdown), "Tipi latte", "Dropdown")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Dropdown()
        {

            return Ok(this.tipiLatteService.DropDown());

        }

        [ViewItem(nameof(Save), "Tipi latte", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] TipoLatteDto model)
        {

            if (model.Id == 0)
                return Ok(this.tipiLatteService.Create(model));
            else
                return Ok(this.tipiLatteService.Update(model));

        }


        [ViewItem(nameof(Delete), "Tipi latte", "Rimozione")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {

            this.tipiLatteService.Delete(id);
            return Ok();

        }

        #endregion

    }
}
