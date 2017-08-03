using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using LatteMarche.Application.Utenti.Interfaces;

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
            //List<string> users = new List<string>();

            //users.Add("pippo");

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

        #endregion


    }
}
