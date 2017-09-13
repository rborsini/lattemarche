using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using LatteMarche.Application.TipiProfilo.Interfaces;
using LatteMarche.Application.TipiProfilo.Dtos;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    public class TipiProfiloController: ApiController
    {
        private ITipiProfiloService tipiProfiloService;

        public TipiProfiloController(ITipiProfiloService tipiProfiloService)
        {
            this.tipiProfiloService = tipiProfiloService;
        }

        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok((this.tipiProfiloService).Details(id));
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
                var tipiLatte = this.tipiProfiloService.Index();
                return Ok(tipiLatte);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }
    }
}
