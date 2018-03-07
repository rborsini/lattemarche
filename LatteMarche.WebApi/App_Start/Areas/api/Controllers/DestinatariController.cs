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

        [HttpGet]
        [HttpPost]
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

        #endregion

    }
}
