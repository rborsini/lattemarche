using LatteMarche.Application.Mobile.Dtos;
using LatteMarche.Application.Mobile.Interfaces;
using LatteMarche.Application.Trasbordi.Dtos;
using LatteMarche.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class TrasbordiController : ApiController
    {
        #region Constants

        private const string PAGE_NAME = "Trasbordi";

        #endregion

        #region Fields

        private LatteMarche.Application.Mobile.Interfaces.ITrasbordiService trasbordiMobileService;
        private LatteMarche.Application.Trasbordi.Interfaces.ITrasbordiService trasbordiService;

        #endregion

        #region Constructor

        public TrasbordiController(
            LatteMarche.Application.Mobile.Interfaces.ITrasbordiService trasbordiMobileService,
            LatteMarche.Application.Trasbordi.Interfaces.ITrasbordiService trasbordiService
            )
        {
            this.trasbordiMobileService = trasbordiMobileService;
            this.trasbordiService = trasbordiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Search), PAGE_NAME, "Ricerca")]
        [HttpGet]
        public IHttpActionResult Search([FromUri] TrasbordiSearchDto searchDto)
        {
            try
            {
                var list = this.trasbordiService.Search(searchDto);
                return Ok(list);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [ViewItem(nameof(Details), PAGE_NAME, "Dettaglio")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            var dto = this.trasbordiService.Details(id);
            return Ok(dto);
        }

        [ViewItem(nameof(Push), PAGE_NAME, "Caricamento")]
        [HttpPost]
        public IHttpActionResult Push([FromBody] LatteMarche.Application.Mobile.Dtos.TrasbordoDto trasbordo)
        {

            var dto = this.trasbordiMobileService.Push(trasbordo);
            return Ok(dto);

        }

        [ViewItem(nameof(Pull), PAGE_NAME, "Recupero")]
        [HttpGet]
        public IHttpActionResult Pull(string imei)
        {

            var model = this.trasbordiMobileService.Pull(imei);
            return Ok(model);

        }

        [ViewItem(nameof(Close), PAGE_NAME, "Chiusura")]
        [HttpPost]
        public IHttpActionResult Close([FromUri] long id)
        {
            this.trasbordiMobileService.Close(id);
            return Ok();

        }

        #endregion
    }
}