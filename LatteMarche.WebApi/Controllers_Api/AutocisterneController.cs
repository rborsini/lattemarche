using LatteMarche.Application.Autocisterne.Dtos;
using LatteMarche.Application.Autocisterne.Interfaces;
using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class AutocisterneController : ApiController
    {
        #region Fields

        private IAutocisterneService service;

        #endregion

        #region Constructors

        public AutocisterneController(IAutocisterneService service)
        {
            this.service = service;
        }

        #endregion

        #region Methods

        //[ViewItem(nameof(Index), "Autocisterne", "Lista")]
        //[HttpGet]
        //public IHttpActionResult Index()
        //{
        //    try
        //    {

        //        var autocisterne = this.service.Index();
        //        return Ok(autocisterne);
        //    }
        //    catch (Exception exc)
        //    {
        //        return InternalServerError(exc);
        //    }

        //}

        //[ViewItem(nameof(Details), "Autocisterne", "Dettaglio")]
        //[HttpGet]
        //public IHttpActionResult Details(int id)
        //{
        //    try
        //    {
        //        return Ok(this.service.Details(id));
        //    }
        //    catch (Exception exc)
        //    {
        //        return InternalServerError(exc);
        //    }

        //}

        [ViewItem(nameof(Dropdown), "Autocisterne", "Dropdown")]
        [HttpGet]
        public IHttpActionResult Dropdown()
        {
            try
            {
                return Ok(this.service.DropDown());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        //[ViewItem(nameof(Save), "Autocisterne", "Salvataggio")]
        //[HttpPost]
        //public IHttpActionResult Save([FromBody] AutocisternaDto model)
        //{
        //    try
        //    {
        //        if (model.Id == 0)
        //            return Ok(this.service.Create(model));
        //        else
        //            return Ok(this.service.Update(model));
        //    }
        //    catch (Exception exc)
        //    {
        //        return InternalServerError(exc);
        //    }

        //}

        //[ViewItem(nameof(Delete), "Autocisterne", "Rimozione")]
        //[HttpDelete]
        //public IHttpActionResult Delete(int id)
        //{
        //    try
        //    {
        //        this.service.Delete(id);
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return InternalServerError(e);
        //    }
        //}

        #endregion

    }
}