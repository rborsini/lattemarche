using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Application.Comuni.Dtos;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.Common.Dtos;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
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

        [ViewItem(nameof(Index), "Comuni", "Lista")]
        [HttpGet]
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

        [ViewItem(nameof(Details), "Comuni", "Dettaglio")]
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

        [ViewItem(nameof(Search), "Comuni", "Ricerca")]
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

        [ViewItem(nameof(Province), "Comuni", "Province")]
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public IHttpActionResult Province()
        {
            try
            {
                var province = ((IComuniService)this.comuniService).GetProvince();
                DropDownDto dropDown = new DropDownDto();
                foreach (var prov in province)
                {
                    dropDown.Items.Add(new DropDownItem() { Value = prov, Text = prov });
                }
                return Ok(dropDown.Items);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        #endregion

    }
}
