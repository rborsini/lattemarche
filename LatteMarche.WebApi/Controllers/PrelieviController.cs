using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Core.Models;
using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;
using RB.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.UI;

namespace LatteMarche.WebApi.Controllers
{
    [MvcCustomAuthorize]
    [MvcActionFilter]
    [MvcExceptionFilter]
    public class PrelieviController: Controller
    {

        private IPrelieviLatteService prelieviLatteService;

        public PrelieviController(IPrelieviLatteService prelieviLatteService)
        {
            this.prelieviLatteService = prelieviLatteService;
        }

        [ViewItem(nameof(Index), "Prelievi", "Lista")]
        [ViewItem("Aggiungi", "Prelievi", "Aggiungi")]
        [ViewItem("Modifica", "Prelievi", "Modifica")]
        [ViewItem("Rimuovi", "Prelievi", "Rimuovi")]
        public ActionResult Index()
        {           
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Excel(string idAllevamento = "", string dal = "", string al = "")
        {
            var prelievi = this.prelieviLatteService.Search(new PrelieviLatteSearchDto()
            {
                idAllevamento = String.IsNullOrEmpty(idAllevamento) || idAllevamento == "undefined" || idAllevamento == "0" ? (int?)null : Convert.ToInt32(idAllevamento),
                DataPeriodoInizio = String.IsNullOrEmpty(dal) ? (DateTime?)null : new DateHelper().ConvertToDateTime(dal),
                DataPeriodoFine = String.IsNullOrEmpty(al) ? (DateTime?)null : new DateHelper().ConvertToDateTime(al),
            });

            RB.Excel.ExcelMaker maker = new RB.Excel.ExcelMaker();

            byte[] content = maker.Make<V_PrelievoLatte>(prelievi);

            return File(content, "application/vnd.ms-excel", "prelievi.xls");

        }

    }

}
