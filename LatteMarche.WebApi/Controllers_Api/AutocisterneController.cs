using System;
using System.Web.Http;
using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.Trasportatori.Dtos;
using LatteMarche.Application.Trasportatori.Interfaces;

namespace LatteMarche.WebApi.Controllers_Api
{
    //[ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
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

        [ViewItem(nameof(Dropdown), "Autocisterne", "Dropdown")]
        [HttpGet]
        public IHttpActionResult Dropdown(int? idTrasportatore)
        {
            try
            {
                return Ok(this.autocisterneService.DropDown(idTrasportatore));
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

        [ViewItem(nameof(Save), "Autocisterne", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] AutocisternaDto model)
        {
            try
            {
                if(model.Id == 0)
                    return Ok(this.autocisterneService.Create(model));
                else
                    return Ok(this.autocisterneService.Update(model));
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
