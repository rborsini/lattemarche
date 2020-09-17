using LatteMarche.Application.Acquirenti.Interfaces;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.AnalisiLatte.Interfaces;
using LatteMarche.Application.Cessionari.Interfaces;
using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Application.Destinatari.Interfaces;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Application.Trasportatori.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WeCode.MVC.Attributes;

namespace LatteMarche.WebApi.Controllers_Api
{

    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class DropDownsController : ApiController
    {
        #region Fields

        private IAllevamentiService allevamentiService;
        private IAcquirentiService acquirentiService;
        private ICessionariService cessionariService;
        private IComuniService comuniService;
        private IDestinatariService destinatariService;
        private ILaboratoriAnalisiService laboratoriAnalisiService;
        private ITipiProfiloService tipiProfiloService;
        private ITipiLatteService tipiLatteService;
        private ITrasportatoriService trasportatoriService;
        private IUtentiService utentiService;


        #endregion

        #region Constructors

        public DropDownsController(
                IAllevamentiService allevamentiService,
                IAcquirentiService acquirentiService,
                ICessionariService cessionariService,
                IComuniService comuniService,
                IDestinatariService destinatariService,
                ILaboratoriAnalisiService laboratoriAnalisiService,
                ITipiProfiloService tipiProfiloService,
                ITipiLatteService tipiLatteService,
                ITrasportatoriService trasportatoriService,
                IUtentiService utentiService
            )
        {
            this.allevamentiService = allevamentiService;
            this.acquirentiService = acquirentiService;
            this.cessionariService = cessionariService;
            this.comuniService = comuniService;
            this.destinatariService = destinatariService;
            this.laboratoriAnalisiService = laboratoriAnalisiService;
            this.tipiProfiloService = tipiProfiloService;
            this.tipiLatteService = tipiLatteService;
            this.trasportatoriService = trasportatoriService;
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "DropDowns", "Lista")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Index(string keys)
        {
            var model = new Dictionary<string, DropDownDto>();
            var list = keys.Split('|').ToList();

            var utente = this.utentiService.Details(User.Identity.Name);

            if (list.Contains("allevatori"))
                model.Add("allevatori", this.allevamentiService.DropDown(utente.Id));

            if (list.Contains("acquirenti"))
                model.Add("acquirenti", this.acquirentiService.DropDown(utente.Id));

            if (list.Contains("cessionari"))
                model.Add("cessionari", this.cessionariService.DropDown(utente.Id));

            if (list.Contains("destinatari"))
                model.Add("destinatari", this.destinatariService.DropDown(utente.Id));

            if (list.Contains("laboratoriAnalisi"))
                model.Add("laboratoriAnalisi", this.laboratoriAnalisiService.DropDown());

            if (list.Contains("profili"))
                model.Add("profili", this.tipiProfiloService.DropDown());

            if (list.Contains("province"))
            {
                var province = this.comuniService.GetProvince();
                DropDownDto dropDown = new DropDownDto();
                foreach (var prov in province)
                {
                    dropDown.Items.Add(new DropDownItem() { Value = prov, Text = prov });
                }
                model.Add("province", dropDown);
            }                

            if (list.Contains("tipiLatte"))
                model.Add("tipiLatte", this.tipiLatteService.DropDown());

            if (list.Contains("trasportatori"))
                model.Add("trasportatori", this.trasportatoriService.DropDown());
                
            return Ok(model);
        }

        #endregion
    }
}