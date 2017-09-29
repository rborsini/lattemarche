using System;
using System.Web.Http;
using LatteMarche.Application.Trasportatori.Interfaces;
using LatteMarche.Application.Trasportatori.Dtos;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.Application.Giri.Dtos;
using LatteMarche.Application.Utenti;
using Newtonsoft.Json.Linq;
using LatteMarche.WebApi.Attributes;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class TrasportatoriController : ApiController
    {

        #region Fields

        private ITrasportatoriService trasportatoriService;
        private IGiriService giriService;

        #endregion

        #region Constructors

        public TrasportatoriController(ITrasportatoriService trasportatoriService, IGiriService giriService)
		{
            this.trasportatoriService = trasportatoriService;
            this.giriService = giriService;
        }

        #endregion

        #region Methods

        [HttpGet]
        [HttpPost]
        public IHttpActionResult Index()
        {
            try
            {
                var trasportatori = this.trasportatoriService.Index();      
                return Ok(trasportatori);
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
                TrasportatoreDto trasportatore = this.trasportatoriService.Details(id);
                trasportatore.Giri = giriService.GetGiriOfTrasportatore(id);
                return Ok(trasportatore);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }
        
        #endregion


    }
}
