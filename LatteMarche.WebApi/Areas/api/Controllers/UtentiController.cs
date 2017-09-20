using System;
using System.Web.Http;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti;
using Newtonsoft.Json.Linq;
using LatteMarche.WebApi.Attributes;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
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
        [HttpPost]
        public IHttpActionResult Index()
        {
            try
            {


                var users = this.utentiService.Index();

                //DataTableResult<UtenteDto> result = new DataTableResult<UtenteDto>();

                //result.meta.page = 1;
                //result.meta.pages = users.Count / 10;
                //result.meta.perpage = 10;
                //result.meta.total = users.Count;
                //result.meta.sort = "asc";
                //result.meta.field = "Nome";

                //result.data = users;

                //return Ok(result);                

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
