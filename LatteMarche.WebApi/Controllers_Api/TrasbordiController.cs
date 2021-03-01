using LatteMarche.Application.Mobile.Dtos;
using LatteMarche.Application.Mobile.Interfaces;
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

        private ITrasbordiService trasbordiService;

        #endregion

        #region Constructor

        public TrasbordiController(ITrasbordiService trasbordiService)
        {
            this.trasbordiService = trasbordiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Push), PAGE_NAME, "Caricamento")]
        [HttpPost]
        public IHttpActionResult Push([FromBody] TrasbordoDto trasbordo)
        {

            var dto = this.trasbordiService.Push(trasbordo);
            return Ok(dto);

        }

        [ViewItem(nameof(Pull), PAGE_NAME, "Recupero")]
        [HttpGet]
        public IHttpActionResult Pull(string imei)
        {

            var model = this.trasbordiService.Pull(imei);
            return Ok(model);

        }

        [ViewItem(nameof(Close), PAGE_NAME, "Chiusura")]
        [HttpPost]
        public IHttpActionResult Close([FromUri] long id)
        {
            this.trasbordiService.Close(id);
            return Ok();

        }

        #endregion
    }
}