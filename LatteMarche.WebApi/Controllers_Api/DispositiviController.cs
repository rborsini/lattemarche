using LatteMarche.Application.Dispositivi.Dtos;
using LatteMarche.Application.Dispositivi.Interfaces;
using LatteMarche.WebApi.Filters;
using log4net;
using System;
using System.Web.Http;
using WeCode.JQueryDataTable;
using WeCode.JQueryDataTable.Models;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class DispositiviController : ApiController
    {
        #region Fields

        private IDispositiviService dispositiviService;

        private static ILog log = LogManager.GetLogger(typeof(DispositiviController));

        #endregion

        #region Constructors

        public DispositiviController(IDispositiviService dispositiviService)
        {
            this.dispositiviService = dispositiviService;
        }

        #endregion

        #region Methods


        [ViewItem(nameof(Search), "Dispositivi", "Ricerca")]
        [HttpPost]
        public IHttpActionResult Search([FromBody] DataTableAjaxPostModel filterModel, [FromUri] DispositiviSearchDto searchDto)
        {
            searchDto.FullText = filterModel.Search.Value;

            searchDto = JQueryDataTableHelper.Merge<DispositiviSearchDto>(filterModel, searchDto);
            var pagedResult = this.dispositiviService.Search(searchDto);

            return Ok(JQueryDataTableHelper.ConvertToResultModel<DispositivoMobileDto>(pagedResult));
        }

        [ViewItem(nameof(Details), "Dispositivi", "Dettaglio")]
        [HttpGet]
        public IHttpActionResult Details(string id)
        {
            try
            {
                return Ok(this.dispositiviService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Update), "Dispositivi", "Aggiornamento")]
        [HttpPut]
        public IHttpActionResult Update([FromBody] DispositivoMobileDto dispositivo)
        {
            try
            {
                var model = this.dispositiviService.Update(dispositivo);
                return Ok(model);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }


        #endregion

    }
}