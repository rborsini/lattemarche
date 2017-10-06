using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using LatteMarche.Application.TipiProfilo.Interfaces;
using LatteMarche.Application.TipiProfilo.Dtos;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class TipiProfiloController: ApiController
    {

        #region Fields

        private ITipiProfiloService tipiProfiloService;

        #endregion

        #region Constructors

        public TipiProfiloController(ITipiProfiloService tipiProfiloService)
        {
            this.tipiProfiloService = tipiProfiloService;
        }

        #endregion

        #region Methods

        [HttpGet]
        [HttpPost]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public IHttpActionResult Index()
        {
            try
            {
                return Ok(this.tipiProfiloService.Index());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.tipiProfiloService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        #endregion

    }
}
