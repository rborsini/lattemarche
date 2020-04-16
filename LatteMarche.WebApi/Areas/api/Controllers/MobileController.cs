using AutoMapper;
using LatteMarche.Application.Acquirenti.Interfaces;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.Destinatari.Interfaces;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.Application.LaboratoriAnalisi.Interfaces;
using LatteMarche.Application.Mobile.Dtos;
using LatteMarche.Application.Mobile.Interfaces;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Core.Models;
using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;
using LatteMarche.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace LatteMarche.WebApi.Areas.api.Controllers
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
        public IHttpActionResult Register([FromBody] DeviceInfoDto deviceInfo)
        {
            try
            {
                this.mobileService.Register(deviceInfo);
                return Ok();
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