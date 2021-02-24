using System;
using System.Web.Http;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.AnalisiLatte.Interfaces;
using LatteMarche.Application.AnalisiLatte.Dtos;
using RB.Excel;
using WeCode.MVC.Controllers;
using WeCode.MVC.Attributes;
using LatteMarche.Application.Utenti.Interfaces;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class AnalisiController : ApiFileController
    {

        #region Fields

        private IAnalisiService analisiService;
        private IUtentiService utentiService;

        #endregion

        #region Constructors

        public AnalisiController(IAnalisiService analisiService, IUtentiService utentiService)
        {
            this.analisiService = analisiService;
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Categorie), "Analisi", "Dropdown Categorie")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Categorie()
        {
            return Ok(this.analisiService.DropDown());
        }

        [ViewItem(nameof(Synch), "Analisi", "Sincronizzazione")]
        [HttpPost]
        public IHttpActionResult Synch()
        {
            var reports = this.analisiService.Synch();
            return Ok(reports);
        }

        [ViewItem(nameof(Search), "Analisi", "Ricerca")]
        [HttpGet]
        public IHttpActionResult Search([FromUri] AnalisiSearchDto searchDto)
        {
            var utente = this.utentiService.GetByUsername(User.Identity.Name);
            return Ok(this.analisiService.Search(searchDto, utente.Id));
        }

        [ViewItem(nameof(Excel), "Analisi", "Excel")]
        [HttpGet]
        public IHttpActionResult Excel([FromUri] AnalisiSearchDto searchDto)
        {
            var utente = this.utentiService.GetByUsername(User.Identity.Name);
            var list = this.analisiService.Search(searchDto, utente.Id);

            byte[] content = LatteMarche.WebApi.Helpers.ExcelAnalisiLatteHelper.MakeExcel(list);

            return File(content, "analisi latte.xlsx", "application/vnd.ms-excel");

        }

        #endregion


    }
}
