using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Core.Models;
using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;
using LatteMarche.WebApi.Models;
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
        private IUtentiService utentiService;


        public PrelieviController(IPrelieviLatteService prelieviLatteService, IUtentiService utentiService)
        {
            this.prelieviLatteService = prelieviLatteService;
            this.utentiService = utentiService;
        }

        [ViewItem(nameof(Index), "Prelievi", "Lista")]
        [ViewItem("Aggiungi", "Prelievi", "Aggiungi")]
        [ViewItem("Modifica", "Prelievi", "Modifica")]
        [ViewItem("Rimuovi", "Prelievi", "Rimuovi")]
        [ViewItem("RicercaAllevatore", "Prelievi", "Ricerca per Allevatore")]
        [ViewItem("RicercaTrasportatore", "Prelievi", "Ricerca per Trasportatore")]
        [ViewItem("RicercaAcquirente", "Prelievi", "Ricerca per Acquirente")]
        [ViewItem("RicercaDestinatario", "Prelievi", "Ricerca per Destinatario")]
    
        public ActionResult Index()
        {           
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Excel(string idAllevamento = "", string idTrasportatore = "", string idAcquirente = "", string idDestinatario = "", string dal = "", string al = "")
        {

            UtenteDto utente = this.utentiService.GetByUsername(User.Identity.Name);

            switch (utente.IdProfilo)
            {
                case 3:     // profilo allevatore
                    idAllevamento = utente.Id.ToString();
                    break;
                case 5:     // profilo trasportatore
                    idTrasportatore = utente.Id.ToString();
                    break;
                case 7:     // profilo acquirente
                    idAcquirente = utente.Id.ToString();
                    break;
                case 6:     // profilo destinatario
                    idDestinatario = utente.Id.ToString();
                    break;
            }

            var prelievi = this.prelieviLatteService.Search(new PrelieviLatteSearchDto()
            {
                idAllevamento = String.IsNullOrEmpty(idAllevamento) || idAllevamento == "undefined" || idAllevamento == "0" ? (int?)null : Convert.ToInt32(idAllevamento),
                idTrasportatore = String.IsNullOrEmpty(idTrasportatore) || idTrasportatore == "undefined" || idTrasportatore == "0" ? (int?)null : Convert.ToInt32(idTrasportatore),
                idAcquirente = String.IsNullOrEmpty(idAcquirente) || idAcquirente == "undefined" || idAcquirente == "0" ? (int?)null : Convert.ToInt32(idAcquirente),
                idDestinatario = String.IsNullOrEmpty(idDestinatario) || idDestinatario == "undefined" || idDestinatario == "0" ? (int?)null : Convert.ToInt32(idDestinatario),
                DataPeriodoInizio = String.IsNullOrEmpty(dal) ? (DateTime?)null : new DateHelper().ConvertToDateTime(dal),
                DataPeriodoFine = String.IsNullOrEmpty(al) ? (DateTime?)null : new DateHelper().ConvertToDateTime(al),
            });

            byte[] content = LatteMarche.WebApi.Helpers.ExcelHelper.MakeExcelTot(prelievi);


            return File(content, "application/vnd.ms-excel", "prelievi.xls");

        }


    }

}
