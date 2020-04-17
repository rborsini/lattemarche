using LatteMarche.Application.Allevamenti.Interfaces;
using System;
using System.Web.Http;

namespace LatteMarche.WebApi.Areas.api.Controllers
{

    public class AllevatoriController : ApiController
    {

        #region Fields

        private IAllevatoriService allevatoriService;

		#endregion

		#region Constructors

		public AllevatoriController(IAllevatoriService allevatoriService)
		{
            this.allevatoriService = allevatoriService;
		}

        #endregion

        #region Methods

        [HttpGet]
        [HttpPost]
        public IHttpActionResult Index()
        {
            try
            {    
                return Ok(this.allevatoriService.Index());
            }
            catch(Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.allevatoriService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        #endregion


    }
}
