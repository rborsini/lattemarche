using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using LatteMarche.Application.Documenti.Interfaces;
using LatteMarche.Application.Documenti.Dtos;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class DocumentiController : ApiController
    {

        #region Fields

        private IDocumentiService documentiService;

        #endregion

        #region Constructors

        public DocumentiController(IDocumentiService documentiService)
        {
            this.documentiService = documentiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Documenti", "Lista")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            try
            {
                return Ok(this.documentiService.Index());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        #endregion

    }
}
