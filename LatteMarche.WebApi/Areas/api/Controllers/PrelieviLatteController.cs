using System;
using System.Web.Http;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Application.PrelieviLatte.Dtos;
using Newtonsoft.Json.Linq;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class PrelieviLatteController : ApiController
    {

        #region Fields

        private IPrelieviLatteService prelieviLatteService;      

        #endregion

        #region Constructors

        public PrelieviLatteController(IPrelieviLatteService prelieviLatteService)
		{
            this.prelieviLatteService = prelieviLatteService;     
        }

        #endregion

        #region Methods

        [HttpGet]
        [HttpPost]
        public IHttpActionResult Index()
        {
            try
            {
                return Ok(this.prelieviLatteService.Index());
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
                return Ok(this.prelieviLatteService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpPut]
        public IHttpActionResult Update([FromBody] PrelievoLatteDto model)
        {
            try
            {
                var users = this.prelieviLatteService.Update(model);
                return Ok(model);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] PrelievoLatteDto model)
        {
            try
            {
                return Ok(this.prelieviLatteService.Create(model));
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
                this.prelieviLatteService.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public IHttpActionResult Search(int idAllevamento,
            DateTime? DataPeriodoInizio=null,
            DateTime? DataPeriodoFine=null
            )
        {
            //possibilità di mettere altri parametri come le date periodo prelievo
            try
            {
                return Ok(this.prelieviLatteService.Search(new PrelieviLatteSearchDto() {
                    idAllevamento = idAllevamento,
                    DataPeriodoInizio = DataPeriodoInizio,
                    DataPeriodoFine =DataPeriodoFine
                }));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        #endregion


    }
}
