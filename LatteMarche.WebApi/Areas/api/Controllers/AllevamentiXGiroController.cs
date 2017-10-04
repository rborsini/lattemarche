using System;
using System.Web.Http;
using LatteMarche.Application.AllevamentiXGiro.Interfaces;
using LatteMarche.Application.AllevamentiXGiro.Dtos;
using Newtonsoft.Json.Linq;
using LatteMarche.WebApi.Attributes;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class AllevamentiXGiroController : ApiController
    {

        #region Fields

        private IAllevamentiXGiroService allevamentiXGiroService;      

        #endregion

        #region Constructors

        public AllevamentiXGiroController(IAllevamentiXGiroService allevamentiXGiroService)
		{
            this.allevamentiXGiroService = allevamentiXGiroService;     
        }

        #endregion

        #region Methods

        [HttpGet]
        public IHttpActionResult Index()
        {//PENSARE SE TOGLIERE. CARICA TUTTA LA VIEW CHE ESSENDO PRODOTTO CARTESIANO è COMPOSTA DA 5000 RIGHE E QUESTO METODO NON DOVREBBE MAI ESSERE UTILIZZATO
            try
            {
                return Ok(this.allevamentiXGiroService.Index());
            }
            catch(Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                return Ok(this.allevamentiXGiroService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpGet]
        public IHttpActionResult Index(int idGiro)
        {
            try
            {
                return Ok(this.allevamentiXGiroService.GetByGiro(idGiro));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        #endregion


    }
}
