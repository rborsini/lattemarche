using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Application.Comuni.Dtos;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    public class ComuniController : ApiController
    {
        private IComuniService comuniService;

        public ComuniController(IComuniService comuniService)
        {
            this.comuniService = comuniService;
        }

        [HttpGet]
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
        public IHttpActionResult Search(string provincia)
        {
            try
            {
                return Ok(this.comuniService.Search(provincia));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [HttpGet]
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
        public IHttpActionResult Province()
        {
            try
            {
                return Ok(((IComuniService)this.comuniService).getProvince());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }
    }
}
