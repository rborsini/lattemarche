using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using LatteMarche.Application.TipiLatte.Interfaces;
using LatteMarche.Application.TipiLatte.Dtos;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    public class TipiLatteController : ApiController
    {
        private ITipiLatteService tipiLatteService;

        public TipiLatteController(ITipiLatteService tipiLatteService)
        {
            this.tipiLatteService = tipiLatteService;
        }

        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok((this.tipiLatteService).Details(id));
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
                var tipiLatte = this.tipiLatteService.Index();
                return Ok(tipiLatte);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }
    }
}
