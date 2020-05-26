using LatteMarche.Application.AnalisiLatte.Dtos;
using LatteMarche.Application.AnalisiLatte.Interfaces;
using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

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
        public IHttpActionResult Index()
        {
            try
            {

                var laboratori = this.service.Index();
                return Ok(laboratori);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Details), "Laboratori Analisi", "Dettaglio")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.service.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Dropdown), "Laboratori Analisi", "Dropdown")]
        [HttpGet]
        public IHttpActionResult Dropdown()
        {
            try
            {
                return Ok(this.service.DropDown());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [ViewItem(nameof(Save), "Laboratori Analisi", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] LaboratorioAnalisiDto model)
        {
            try
            {
                if (model.Id == 0)
                    return Ok(this.service.Create(model));
                else
                    return Ok(this.service.Update(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Delete), "Laboratori Analisi", "Rimozione")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                this.service.Delete(id);
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