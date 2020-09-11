using System;
using System.Web.Http;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.AnalisiLatte.Interfaces;
using LatteMarche.Application.AnalisiLatte.Dtos;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
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

                var reports = this.analisiService.Synch();
                return Ok(reports);

        }

        [ViewItem(nameof(Search), "Analisi latte", "Ricerca")]
        [HttpGet]
        public IHttpActionResult Search([FromUri] AnalisiSearchDto searchDto)
        {

                return Ok(this.analisiService.Search(searchDto));

        }

        #endregion


    }
}
