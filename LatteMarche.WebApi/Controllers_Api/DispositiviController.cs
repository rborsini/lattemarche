using LatteMarche.Application.Dispositivi.Dtos;
using LatteMarche.Application.Dispositivi.Interfaces;
using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;
using System;
using System.Web.Http;

namespace LatteMarche.WebApi.Controllers_Api
{
    //[ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class DispositiviController : ApiController
    {
        #region Fields

        private IDispositiviService dispositiviService;

        #endregion

        #region Constructors

        public DispositiviController(IDispositiviService dispositiviService)
        {
            this.dispositiviService = dispositiviService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Dispositivi", "Ricerca")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            try
            {
                var list = this.dispositiviService.Index();
                return Ok(list);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
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