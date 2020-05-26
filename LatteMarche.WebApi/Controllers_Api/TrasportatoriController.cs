using LatteMarche.Application.Trasportatori.Interfaces;
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
        public IHttpActionResult Dropdown()
        {
            try
            {
                return Ok(this.trasportatoriService.DropDown());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        #endregion

    }
}