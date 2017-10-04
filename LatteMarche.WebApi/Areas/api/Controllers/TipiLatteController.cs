using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using LatteMarche.Application.TipiLatte.Interfaces;
using LatteMarche.Application.TipiLatte.Dtos;
using LatteMarche.WebApi.Attributes;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class TipiLatteController : ApiController
    {

        #region Fields

        private ITipiLatteService tipiLatteService;

        #endregion

        #region Constructors

        public TipiLatteController(ITipiLatteService tipiLatteService)
        {
            this.tipiLatteService = tipiLatteService;
        }

        #endregion

        #region Methods

        [HttpGet]
        public IHttpActionResult Index()
        {
            try
            {
                return Ok(this.tipiLatteService.Index());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.tipiLatteService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        #endregion

    }
}
