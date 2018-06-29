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

        [ViewItem(nameof(Index), "Giri", "Lista")]
        [HttpGet]
        public IHttpActionResult Index(int idTrasportatore)
        {
            try
            {
                return Ok(this.giriService.GetGiriTrasportatore(idTrasportatore));
            }
            catch(Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Details), "Giri", "Dettaglio")]
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

        [ViewItem(nameof(Update), "Giri", "Aggiornamento")]
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

        [ViewItem(nameof(Create), "Giri", "Creazione")]
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

        [ViewItem(nameof(Delete), "Giri", "Rimozione")]
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
