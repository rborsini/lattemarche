using System;
using System.Web.Http;
using LatteMarche.Application.Allevatori.Interfaces;
using LatteMarche.Application.Allevatori.Dtos;
using LatteMarche.Application.Allevatori;
using Newtonsoft.Json.Linq;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;

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
