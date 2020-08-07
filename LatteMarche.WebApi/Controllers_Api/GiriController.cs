using System;
using System.Web.Http;
using LatteMarche.Application.Giri.Dtos;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.WebApi.Filters;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
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

        //[ViewItem(nameof(Index), "Giri", "Lista")]
        //[HttpGet]
        //public IHttpActionResult Index(int idTrasportatore = -1)
        //{
        //    try
        //    {
        //        if(idTrasportatore != -1)
        //            return Ok(this.giriService.GetGiriTrasportatore(idTrasportatore));
        //        else
        //            return Ok(this.giriService.Index());
        //    }
        //    catch(Exception exc)
        //    {
        //        return InternalServerError(exc);
        //    }

        //}

        [ViewItem(nameof(DropDown), "Giri", "DropDown")]
        [HttpGet]
        public IHttpActionResult DropDown(int idTrasportatore)
        {
            try
            {
                return Ok(this.giriService.DropDown(idTrasportatore));
            }
            catch (Exception exc)
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


        [ViewItem(nameof(Save), "Giri", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] GiroDto model)
        {
            try
            {
                if (model.Id == 0)
                    return Ok(this.giriService.Create(model));
                else
                    return Ok(this.giriService.Update(model));
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
