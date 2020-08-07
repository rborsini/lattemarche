using LatteMarche.Application.Mobile.Dtos;
using LatteMarche.Application.Mobile.Interfaces;
using LatteMarche.WebApi.Filters;
using System;
using System.Web.Http;

namespace LatteMarche.WebApi.Controllers_Api
{
    //[ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class MobileController : ApiController
    {
        #region Fields

        private IMobileService mobileService;

        #endregion

        #region Constructors

        public MobileController(IMobileService mobileService)
        {
            this.mobileService = mobileService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Register), "Mobile", "Registrazione")]
        [HttpPost]
        public IHttpActionResult Register([FromBody] DispositivoDto deviceInfo)
        {
            try
            {
                var dto = this.mobileService.Register(deviceInfo);
                return Ok(dto);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [ViewItem(nameof(Download), "Mobile", "Download")]
        [HttpGet]
        public IHttpActionResult Download(string imei)
        {
            try
            {
                var model = this.mobileService.Download(imei);
                return Ok(model);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Upload), "Mobile", "Upload")]
        [HttpPost]
        public IHttpActionResult Upload([FromBody] UploadDto dto)
        {
            try
            {
                this.mobileService.Upload(dto);
                return Ok();
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        #endregion

    }
}