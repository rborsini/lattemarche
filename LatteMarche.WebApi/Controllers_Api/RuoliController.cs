using System;
using System.Web.Http;
using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Application.Auth.Dtos;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class RuoliController : ApiController
    {

        #region Fields

        private IRuoliService ruoliService;

        #endregion

        #region Constructors

        public RuoliController(IRuoliService ruoliService)
        {
            this.ruoliService = ruoliService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Ruoli", "Lista")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            try
            {
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
