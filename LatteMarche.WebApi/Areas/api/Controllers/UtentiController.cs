using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    public class UtentiController : ApiController
    {

        #region Fields

        private IUtentiService utentiService;

		#endregion

		#region Constructors

		public UtentiController(IUtentiService utentiService)
		{
            this.utentiService = utentiService;
		}

        #endregion

        #region Methods

        [HttpGet]
        public IHttpActionResult Index()
        {
            try
            {
                var users = this.utentiService.Index();
				return Ok(users);                
            }
            catch(Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                var users = this.utentiService.Index();
                return Ok(this.utentiService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpPut]
        public IHttpActionResult Update([FromBody] UtenteDto model)
        {
            try
            {
                var users = this.utentiService.Update(model);
                return Ok(model);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] UtenteDto model)
        {
            try
            {
                var users = this.utentiService.Create(model);
                return Ok(model);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }


        #endregion


    }
}
