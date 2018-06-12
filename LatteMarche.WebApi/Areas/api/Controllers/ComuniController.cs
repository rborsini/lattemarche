using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Application.Comuni.Dtos;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class ComuniController : ApiController
    {

        #region Fields

        private IComuniService comuniService;

        #endregion

        #region Constructors

        public ComuniController(IComuniService comuniService)
        {
            this.comuniService = comuniService;
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
                var comuni = this.comuniService.Index();
                return Ok(comuni);
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
                return Ok((this.comuniService).Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public IHttpActionResult Search(string provincia)
        {
            try
            {
                return Ok(this.comuniService.Search(new ComuniSearchDto() { SiglaProvincia = provincia }));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public IHttpActionResult Province()
        {
            try
            {
                var province = ((IComuniService)this.comuniService).GetProvince();
                Dictionary<string, string> dropDown = new Dictionary<string, string>();
                foreach (var prov in province)
                {
                    dropDown.Add(prov, prov);
                }
                return Ok(dropDown);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        #endregion

    }
}
