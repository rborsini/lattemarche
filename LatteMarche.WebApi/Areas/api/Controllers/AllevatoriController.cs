using System;
using System.Web.Http;
using LatteMarche.Application.Allevatori.Interfaces;
using LatteMarche.Application.Allevatori.Dtos;
using LatteMarche.Application.Allevatori;
using Newtonsoft.Json.Linq;
using LatteMarche.WebApi.Attributes;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class AllevatoriController : ApiController
    {

        #region Fields

        private IAllevatoriService allevatoriService;

		#endregion

		#region Constructors

		public AllevatoriController(IAllevatoriService allevatoriService)
		{
            this.allevatoriService = allevatoriService;
		}

        #endregion

        #region Methods

        [HttpGet]
        [HttpPost]
        public IHttpActionResult Index()
        {
            try
            {


                var allevatori = this.allevatoriService.Index();

                //DataTableResult<UtenteDto> result = new DataTableResult<UtenteDto>();

                //result.meta.page = 1;
                //result.meta.pages = users.Count / 10;
                //result.meta.perpage = 10;
                //result.meta.total = users.Count;
                //result.meta.sort = "asc";
                //result.meta.field = "Nome";

                //result.data = users;

                //return Ok(result);                

                return Ok(allevatori);
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
                return Ok(this.allevatoriService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        #endregion


    }
}
