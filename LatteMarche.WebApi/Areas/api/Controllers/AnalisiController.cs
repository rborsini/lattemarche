using System;
using System.Web.Http;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Allevamenti;
using Newtonsoft.Json.Linq;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.AnalisiLatte.Interfaces;
using LatteMarche.Application.AnalisiLatte.Dtos;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    //[ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class AnalisiController : ApiController
    {

        #region Fields

        private IAnalisiService analisiService;

        #endregion

        #region Constructors

        public AnalisiController(IAnalisiService analisiService)
        {
            this.analisiService = analisiService;
        }

        #endregion

        #region Methods


        [ViewItem(nameof(Synch), "Analisi latte", "Sincronizzazione")]
        [HttpPost]
        public IHttpActionResult Synch()
        {
            try
            {
                this.analisiService.Synch();
                return Ok();
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Search), "Analisi latte", "Ricerca")]
        [HttpGet]
        public IHttpActionResult Search([FromUri] AnalisiSearchDto searchDto)
        {
            try
            {
                return Ok(this.analisiService.Search(searchDto));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        #endregion


    }
}
