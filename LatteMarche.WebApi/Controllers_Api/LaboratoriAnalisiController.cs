using LatteMarche.Application.AnalisiLatte.Dtos;
using LatteMarche.Application.AnalisiLatte.Interfaces;
using LatteMarche.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WeCode.MVC.Attributes;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class LaboratoriAnalisiController : ApiController
    {
        #region Fields

        private ILaboratoriAnalisiService service;

        #endregion

        #region Constructors

        public LaboratoriAnalisiController(ILaboratoriAnalisiService service)
        {
            this.service = service;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Laboratori Analisi", "Lista")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Index()
        {

            var laboratori = this.service.Index();
            return Ok(laboratori);

        }

        [ViewItem(nameof(Details), "Laboratori Analisi", "Dettaglio")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Details(int id)
        {

            return Ok(this.service.Details(id));

        }

        [ViewItem(nameof(Dropdown), "Laboratori Analisi", "Dropdown")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Dropdown()
        {

            return Ok(this.service.DropDown());

        }

        [ViewItem(nameof(Save), "Laboratori Analisi", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] LaboratorioAnalisiDto model)
        {

            if (model.Id == 0)
                return Ok(this.service.Create(model));
            else
                return Ok(this.service.Update(model));

        }

        [ViewItem(nameof(Delete), "Laboratori Analisi", "Rimozione")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            this.service.Delete(id);
            return Ok();

        }

        #endregion

    }
}