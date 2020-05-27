using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
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

        #region Fields

        private IPrelieviLatteService prelieviLatteService;
        private IUtentiService utentiService;

        #endregion

        #region Constructor

        public PrelieviController(IPrelieviLatteService prelieviLatteService, IUtentiService utentiService)
        {
            this.prelieviLatteService = prelieviLatteService;
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

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

        #endregion

    }

}
