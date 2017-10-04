using System;
using System.Web.Http;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Allevamenti;
using Newtonsoft.Json.Linq;
using LatteMarche.WebApi.Attributes;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class AllevamentiController : ApiController
    {

        #region Fields

        private IAllevamentiService allevamentiService;

        #endregion

        #region Constructors

        public AllevamentiController(IAllevamentiService allevamentiService)
        {
            this.allevamentiService = allevamentiService;
        }

        #endregion

        #region Methods

        [HttpGet]
        public IHttpActionResult Index()
        {
            try
            {

                var allevamenti = this.allevamentiService.Index();

                //DataTableResult<UtenteDto> result = new DataTableResult<UtenteDto>();

                //result.meta.page = 1;
                //result.meta.pages = users.Count / 10;
                //result.meta.perpage = 10;
                //result.meta.total = users.Count;
                //result.meta.sort = "asc";
                //result.meta.field = "Nome";

                //result.data = users;

                //return Ok(result);                

                return Ok(allevamenti);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.allevamentiService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpPut]
        public IHttpActionResult Update([FromBody] AllevamentoDto model)
        {
            try
            {
                return Ok(this.allevamentiService.Update(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] AllevamentoDto model)
        {
            try
            {
                return Ok(this.allevamentiService.Create(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                this.allevamentiService.Delete(id);
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
