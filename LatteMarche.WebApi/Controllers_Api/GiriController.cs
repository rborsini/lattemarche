using System;
using System.Web.Http;
using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.Giri.Dtos;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;
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
        private IUtentiService utentiService;

        #endregion

        #region Constructors

        public GiriController(IGiriService giriService, IUtentiService utentiService)
        {
            this.giriService = giriService;
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods


        [ViewItem(nameof(DropDown), "Giri", "DropDown")]
        [HttpGet]
        [ETag]
        public IHttpActionResult DropDown(int? idTrasportatore = (int?)null)
        {
            DropDownDto dropDownDto = null;
            
            if(idTrasportatore.HasValue)
            {
                dropDownDto = this.giriService.DropDownByTrasportatore(idTrasportatore.Value);
            }                
            else
            {
                var utente = this.utentiService.Details(User.Identity.Name);
                dropDownDto = this.giriService.DropDown(utente.Id);
            }

            return Ok(dropDownDto);
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
