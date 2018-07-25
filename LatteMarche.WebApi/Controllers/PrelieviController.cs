using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.WebApi.Attributes;
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
    public class PrelieviController: Controller
    {

        private IPrelieviLatteService prelieviLatteService;

        public PrelieviController(IPrelieviLatteService prelieviLatteService)
        {
            this.prelieviLatteService = prelieviLatteService;
        }

        [ViewItem(nameof(Index), "Prelievi", "Lista")]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Index()
        {           
            return View();
        }

        [ViewItem(nameof(Edit), "Prelievi", "Modifica")]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Edit(long id)
        {
            return View();
        }

        //[ViewItem(nameof(Search), "Prelievi latte", "Ricerca")]
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

            byte[] content = maker.Make<PrelievoLatteDto>(prelievi);

            return File(content, "application/vnd.ms-excel", "prelievi.xls");

        }

    }

}
