using System;
using System.Web.Http;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Application.Auth.Dtos;
using LatteMarche.WebApi.Helpers;
using WeCode.MVC.Attributes;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class RuoliController : ApiController
    {

        #region Fields

        private IRuoliService ruoliService;
        private IAzioniService azioniService;

        #endregion

        #region Constructors

        public RuoliController(IRuoliService ruoliService, IAzioniService azioniService)
        {
            this.ruoliService = ruoliService;
            this.azioniService = azioniService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Ruoli", "Lista")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Index()
        {
            this.azioniService.Synch(ReflectionHelper.GetAzioni());
            return Ok(this.ruoliService.Index());
        }

        [ViewItem(nameof(Details), "Ruoli", "Dettaglio")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Details(int id)
        {

            return Ok(this.ruoliService.Details(id));

        }

        [ViewItem(nameof(Create), "Ruoli", "Creazione")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] RuoloDto model)
        {

            return Ok(this.ruoliService.Create(model));

        }

        [ViewItem(nameof(Update), "Ruoli", "Aggiornamento")]
        [HttpPut]
        public IHttpActionResult Update([FromBody] RuoloDto model)
        {

            return Ok(this.ruoliService.Update(model));

        }

        #endregion

    }
}
