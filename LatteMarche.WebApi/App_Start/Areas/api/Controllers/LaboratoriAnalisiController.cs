using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using LatteMarche.Application.LaboratoriAnalisi.Interfaces;
using LatteMarche.Application.LaboratoriAnalisi.Dtos;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class LaboratoriAnalisiController: ApiController
    {

        #region Fields

        private ILaboratoriAnalisiService laboratoriAnalisiService;

        #endregion

        #region Constructors

        public LaboratoriAnalisiController(ILaboratoriAnalisiService laboratoriAnalisiService)
        {
            this.laboratoriAnalisiService = laboratoriAnalisiService;
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
                return Ok(this.laboratoriAnalisiService.Index());
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
                return Ok(this.laboratoriAnalisiService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        #endregion

    }
}
