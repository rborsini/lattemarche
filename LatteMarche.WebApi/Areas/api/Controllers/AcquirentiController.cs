using System;
using System.Web.Http;
using LatteMarche.Application.Acquirenti.Interfaces;
using LatteMarche.Application.Acquirenti.Dtos;
using Newtonsoft.Json.Linq;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class AcquirentiController : ApiController
    {

        #region Fields

        private IAcquirentiService acquirentiService;

        #endregion

        #region Constructors

        public AcquirentiController(IAcquirentiService acquirentiService)
        {
            this.acquirentiService = acquirentiService;
        }

        #endregion

        #region Methods

        [HttpGet]
        [HttpPost]
        public IHttpActionResult Index()
        {
            try
            {

                var acquirenti = this.acquirentiService.Index();           
                return Ok(acquirenti);
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
                return Ok(this.acquirentiService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpPut]
        public IHttpActionResult Update([FromBody] AcquirenteDto model)
        {
            //E' previsto che si possa fare?
            try
            {
                return Ok(this.acquirentiService.Update(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] AcquirenteDto model)
        {
            //E' previsto che si possa fare?
            try
            {
                return Ok(this.acquirentiService.Create(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            //E' previsto che si possa fare?
            try
            {
                this.acquirentiService.Delete(id);
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
