using LatteMarche.Application.Mobile.Dtos;
using LatteMarche.Application.Mobile.Interfaces;
using LatteMarche.WebApi.Filters;
using log4net;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Web.Http;

namespace LatteMarche.WebApi.Controllers_Api
{
    //[ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class MobileController : ApiController
    {
        #region Constants

        private const string PAGE_NAME = "Mobile";

        #endregion

        #region Fields

        private IMobileService mobileService;

        private static ILog log = LogManager.GetLogger(typeof(MobileController));

        #endregion

        #region Constructors

        public MobileController(IMobileService mobileService)
        {
            this.mobileService = mobileService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Register), PAGE_NAME, "Registrazione")]
        [HttpPost]
        public IHttpActionResult Register([FromBody] DispositivoDto deviceInfo)
        {
            try
            {
                var sw = new Stopwatch();

                log.Info($"api/Mobile/Register deviceInfo: {JsonConvert.SerializeObject(deviceInfo)}");

                sw.Start();
                var dto = this.mobileService.Register(deviceInfo);
                sw.Stop();

                log.Info($"api/Mobile/Register [{sw.Elapsed}] dto: {JsonConvert.SerializeObject(dto)}");

                return Ok(dto);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return BadRequest(ex.Message);
            }
        }

        [ViewItem(nameof(Download), PAGE_NAME, "Download")]
        [HttpGet]
        public IHttpActionResult Download(string imei)
        {
            try
            {
                var sw = new Stopwatch();
                log.Info($"api/Mobile/Download imei: {imei}");

                sw.Start();

                var model = this.mobileService.Download(imei);

                sw.Stop();

                log.Info($"api/Mobile/Download [{sw.Elapsed}] model: {JsonConvert.SerializeObject(model)}");

                return Ok(model);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return BadRequest(ex.Message);
            }

        }

        [ViewItem(nameof(Upload), PAGE_NAME, "Upload")]
        [HttpPost]
        public IHttpActionResult Upload([FromBody] UploadDto dto)
        {
            try
            {
                var sw = new Stopwatch();
                log.Info($"api/Mobile/Download dto: {JsonConvert.SerializeObject(dto)}");

                sw.Start();

                this.mobileService.Upload(dto);

                sw.Stop();

                log.Info($"api/Mobile/Download [{sw.Elapsed}]");

                return Ok();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return BadRequest(ex.Message);
            }

        }

        #endregion

    }
}