using LatteMarche.Application.Acquirenti.Interfaces;
using LatteMarche.Application.Auth.Dtos;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Application.Latte.Dtos;
using LatteMarche.Application.Latte.Interfaces;
using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;
using RB.Date;
using System;
using System.Web.Mvc;

namespace LatteMarche.WebApi.Controllers_Web
{
    [MvcCustomAuthorize]
    [MvcActionFilter]
    [MvcExceptionFilter]
    public class PrelieviController: Controller
    {

        private IPrelieviLatteService prelieviLatteService;
        private IUtentiService utentiService;
        private IAcquirentiService acquirentiService;

        public PrelieviController(IPrelieviLatteService prelieviLatteService, IUtentiService utentiService, IAcquirentiService acquirentiService)
        {
            this.prelieviLatteService = prelieviLatteService;
            this.utentiService = utentiService;
            this.acquirentiService = acquirentiService;
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
                    var acquirente = this.acquirentiService.GetByIdUtente(utente.Id);
                    idAcquirente = acquirente != null ? acquirente.Id.ToString() : "";
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
