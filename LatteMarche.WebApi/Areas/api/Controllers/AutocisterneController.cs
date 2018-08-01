using System;
using System.Web.Http;
using LatteMarche.Application.Acquirenti.Interfaces;
using LatteMarche.Application.Acquirenti.Dtos;
using Newtonsoft.Json.Linq;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;
using LatteMarche.Application.Autocisterne.Interfaces;
using LatteMarche.Application.Autocisterne.Dtos;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class AutocisterneController : ApiController
    {

        #region Fields

        private IAutocisterneService autocisterneService;

        #endregion

        #region Constructors

        public AutocisterneController(IAutocisterneService autocisterneService)
        {
            this.autocisterneService = autocisterneService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Autocisterne", "Lista")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            try
            {
                return Ok(this.autocisterneService.Index());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Details), "Autocisterne", "Dettaglio")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.autocisterneService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Update), "Autocisterne", "Aggiornamento")]
        [HttpPut]
        public IHttpActionResult Update([FromBody] AutocisternaDto model)
        {
            try
            {
                return Ok(this.autocisterneService.Update(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Create), "Autocisterne", "Creazione")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] AutocisternaDto model)
        {
            try
            {
                return Ok(this.autocisterneService.Create(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Delete), "Autocisterne", "Rimozione")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                this.autocisterneService.Delete(id);
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
