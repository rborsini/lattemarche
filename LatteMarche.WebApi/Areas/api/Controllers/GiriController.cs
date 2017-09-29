using System;
using System.Web.Http;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.Application.Giri.Dtos;
using LatteMarche.Application.Giri;
using Newtonsoft.Json.Linq;
using LatteMarche.WebApi.Attributes;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class GiriController : ApiController
    {

        #region Fields

        private IGiriService giriService;

		#endregion

		#region Constructors

		public GiriController(IGiriService giriService)
		{
            this.giriService = giriService;
		}

        #endregion

        #region Methods

        [HttpGet]
        [HttpPost]
        public IHttpActionResult Index()
        {
            try
            {


                var users = this.giriService.Index();

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
                return Ok(this.giriService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpPut]
        public IHttpActionResult Update([FromBody] GiroDto model)
        {
            try
            {
                var users = this.giriService.Update(model);
                return Ok(model);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
            
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] GiroDto model)
        {
            try
            {
                var users = this.giriService.Create(model);
                return Ok(model);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        public IHttpActionResult Delete(int id)
        {
            try
            {
                this.giriService.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        #endregion


    }
}
