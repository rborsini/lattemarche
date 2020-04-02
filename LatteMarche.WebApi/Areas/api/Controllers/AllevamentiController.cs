using System;
using System.Web.Http;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Allevamenti;
using Newtonsoft.Json.Linq;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;
using LatteMarche.WebApi.Filters;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class AllevamentiController : ApiController
    {

        #region Fields

        private IAllevamentiService allevamentiService;

        #endregion

        #region Constructors

        public AllevamentiController(IAllevamentiService allevamentiService)
        {
            this.allevamentiService = allevamentiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Allevamenti", "Lista")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            try
            {
                return Ok(this.allevamentiService.Search(new AllevamentiSearchDto()));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Details), "Allevamenti", "Dettaglio")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.allevamentiService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Save), "Allevamenti", "Aggiornamento")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] AllevamentoDto model)
        {
            try
            {
                if(model.Id == 0)
                    return Ok(this.allevamentiService.Create(model));
                else
                    return Ok(this.allevamentiService.Update(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Delete), "Allevamenti", "Rimozione")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                this.allevamentiService.Delete(id);
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
