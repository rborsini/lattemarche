using System;
using System.Web.Http;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.Application.Giri.Dtos;
using LatteMarche.Application.Giri;
using Newtonsoft.Json.Linq;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class GiriController : ApiController
    {

        #region Fields

        private IGiriService giriService;

		#endregion

		#region Constructors

		public GiriController(IGiriService giriService)
		{
            this.giriService = giriService;
		}

        #endregion

        #region Methods

        [HttpGet]
        [HttpPost]
        public IHttpActionResult Index(int idTrasportatore)
        {
            try
            {
                return Ok(this.giriService.GetGiriOfTrasportatore(idTrasportatore));
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
                return Ok(this.giriService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpPut]
        public IHttpActionResult Update([FromBody] GiroDto model)
        {
            try
            {
                return Ok(this.giriService.Update(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
            
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] GiroDto model)
        {
            try
            {
                return Ok(this.giriService.Create(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                this.giriService.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        
        #endregion


    }
}
