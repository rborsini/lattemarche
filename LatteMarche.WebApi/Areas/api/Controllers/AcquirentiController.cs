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

        [ViewItem(nameof(Index), "Acquirenti", "Lista")]
        [HttpGet]
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

        [ViewItem(nameof(Details), "Acquirenti", "Dettaglio")]
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

        [ViewItem(nameof(Update), "Acquirenti", "Aggiornamento")]
        [HttpPut]
        public IHttpActionResult Update([FromBody] AcquirenteDto model)
        {            
            try
            {
                return Ok(this.acquirentiService.Update(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Create), "Acquirenti", "Creazione")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] AcquirenteDto model)
        {
            try
            {
                return Ok(this.acquirentiService.Create(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Save), "Acquirenti", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] AcquirenteDto model)
        {
            try
            {
                if(model.Id == 0)
                    return Ok(this.acquirentiService.Create(model));
                else
                    return Ok(this.acquirentiService.Update(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Delete), "Acquirenti", "Rimozione")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
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
