using System;
using System.Web.Http;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Application.Auth.Dtos;
using LatteMarche.WebApi.Helpers;

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
        public IHttpActionResult Index()
        {
            try
            {
                this.azioniService.Synch(ReflectionHelper.GetAzioni());
                return Ok(this.ruoliService.Index());
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Details), "Ruoli", "Dettaglio")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.ruoliService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [ViewItem(nameof(Create), "Ruoli", "Creazione")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] RuoloDto model)
        {
            try
            {
                return Ok(this.ruoliService.Create(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Update), "Ruoli", "Aggiornamento")]
        [HttpPut]
        public IHttpActionResult Update([FromBody] RuoloDto model)
        {
            try
            {
                return Ok(this.ruoliService.Update(model));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        #endregion

    }
}
