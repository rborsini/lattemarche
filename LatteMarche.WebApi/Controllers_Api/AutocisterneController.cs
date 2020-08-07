using LatteMarche.Application.Autocisterne.Dtos;
using LatteMarche.Application.Autocisterne.Interfaces;
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

        [ViewItem(nameof(Dropdown), "Autocisterne", "Dropdown")]
        [HttpGet]
        public IHttpActionResult Dropdown(int idTrasportatore)
        {
            try
            {
                return Ok(this.service.DropDown(idTrasportatore));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        #endregion

    }
}