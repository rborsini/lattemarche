using LatteMarche.Application.Trasportatori.Interfaces;
using LatteMarche.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WeCode.MVC.Attributes;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class TrasportatoriController : ApiController
    {
        #region Fields

        private ITrasportatoriService trasportatoriService;

        #endregion

        #region Constructor

        public TrasportatoriController(ITrasportatoriService trasportatoriService)
        {
            this.trasportatoriService = trasportatoriService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Dropdown), "Trasportatori", "Dropdown")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Dropdown()
        {

            return Ok(this.trasportatoriService.DropDown());

        }

        #endregion

    }
}