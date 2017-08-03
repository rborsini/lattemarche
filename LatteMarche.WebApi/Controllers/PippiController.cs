using System;
using System.Collections.Generic;
using System.Web.Http;

namespace LatteMarche.WebApi.Controllers
{
    public class PippiController : ApiController
    {
        public PippiController()
        {
        }

		[HttpGet]
		public IHttpActionResult Index()
		{
			List<string> users = new List<string>();

			users.Add("pippo");

			return Ok(users);
        }

    }
}
