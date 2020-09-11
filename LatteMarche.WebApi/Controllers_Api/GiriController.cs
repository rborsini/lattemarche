using System;
using System.Web.Http;
using LatteMarche.Application.Giri.Dtos;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.WebApi.Filters;
using WeCode.MVC.Attributes;

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


        [ViewItem(nameof(DropDown), "Giri", "DropDown")]
        [HttpGet]
        [ETag]
        public IHttpActionResult DropDown(int idTrasportatore)
        {

            return Ok(this.giriService.DropDown(idTrasportatore));

        }

        [ViewItem(nameof(Details), "Giri", "Dettaglio")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Details(int id)
        {

            return Ok(this.giriService.Details(id));

        }


        [ViewItem(nameof(Save), "Giri", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] GiroDto model)
        {

            if (model.Id == 0)
                return Ok(this.giriService.Create(model));
            else
                return Ok(this.giriService.Update(model));

        }

        [ViewItem(nameof(Delete), "Giri", "Rimozione")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {

            this.giriService.Delete(id);
            return Ok();

        }

        #endregion


    }
}
