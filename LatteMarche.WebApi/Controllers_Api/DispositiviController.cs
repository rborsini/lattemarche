using LatteMarche.Application.Dispositivi.Dtos;
using LatteMarche.Application.Dispositivi.Interfaces;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.WebApi.Filters;
using log4net;
using System;
using System.Web.Http;
using WeCode.JQueryDataTable;
using WeCode.JQueryDataTable.Models;
using WeCode.MVC.Attributes;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class DispositiviController : ApiController
    {
        #region Fields

        private IDispositiviService dispositiviService;
        private IUtentiService utentiService;

        private static ILog log = LogManager.GetLogger(typeof(DispositiviController));

        #endregion

        #region Constructors

        public DispositiviController(
            IDispositiviService dispositiviService,
            IUtentiService utentiService
            )
        {
            this.dispositiviService = dispositiviService;
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods


        [ViewItem(nameof(Search), "Dispositivi", "Ricerca")]
        [HttpPost]
        public IHttpActionResult Search([FromBody] DataTableAjaxPostModel filterModel, [FromUri] DispositiviSearchDto searchDto)
        {
            UtenteDto utente = this.utentiService.GetByUsername(User.Identity.Name);

            searchDto.Tenant = utente.Tenant != "all" ? utente.Tenant : null;
            searchDto.FullText = filterModel.Search.Value;

            searchDto = JQueryDataTableHelper.Merge<DispositiviSearchDto>(filterModel, searchDto);
            var pagedResult = this.dispositiviService.Search(searchDto);

            return Ok(JQueryDataTableHelper.ConvertToResultModel<DispositivoMobileDto>(pagedResult));
        }

        [ViewItem(nameof(Details), "Dispositivi", "Dettaglio")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Details(string id)
        {

            return Ok(this.dispositiviService.Details(id));

        }

        [ViewItem(nameof(Update), "Dispositivi", "Aggiornamento")]
        [HttpPut]
        public IHttpActionResult Update([FromBody] DispositivoMobileDto dispositivo)
        {

            var model = this.dispositiviService.Update(dispositivo);
            return Ok(model);

        }

        [ViewItem(nameof(Delete), "Dispositivi", "Rimozione")]
        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {

            this.dispositiviService.Delete(id);
            return Ok();

        }

        #endregion

    }
}