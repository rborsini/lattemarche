using System;
using System.Web.Http;
using LatteMarche.Application.Giri.Dtos;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.Application.Logs.Interfaces;
using LatteMarche.WebApi.Filters;

namespace LatteMarche.WebApi.Controllers_Api
{
    [AllowAnonymous]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class LogsController : ApiController
    {

        #region Fields

        private ILogsService logsService;

		#endregion

		#region Constructors

		public LogsController(ILogsService logsService)
		{
            this.logsService = logsService;
		}

        #endregion

        #region Methods


        [ViewItem(nameof(Clear), "Logs", "Pulizia vecchi record")]
        [HttpPost]
        public IHttpActionResult Clear()
        {
            try
            {
                var date = DateTime.Today.AddDays(-10);
                this.logsService.Delete(date);
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
