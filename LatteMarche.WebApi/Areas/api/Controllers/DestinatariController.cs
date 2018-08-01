using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using LatteMarche.Application.LaboratoriAnalisi.Interfaces;
using LatteMarche.Application.LaboratoriAnalisi.Dtos;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;
using LatteMarche.Application.Destinatari.Interfaces;
using LatteMarche.Application.Destinatari.Dtos;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class DestinatariController: ApiController
    {

        #region Fields

        private IDestinatariService destinatariService;

        #endregion

        #region Constructors

        public DestinatariController(IDestinatariService destinatariService)
        {
            this.destinatariService = destinatariService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Destinatari", "Lista")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            try
            {
                return Ok(this.destinatariService.Index());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Details), "Destinatari", "Dettaglio")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.destinatariService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [ViewItem(nameof(Save), "Destinatari", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] DestinatarioDto model)
        {
            try
            {
                if(model.Id == 0)
                    return Ok(this.destinatariService.Create(model));
                else
                    return Ok(this.destinatariService.Update(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Delete), "Destinatari", "Rimozione")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                this.destinatariService.Delete(id);
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
