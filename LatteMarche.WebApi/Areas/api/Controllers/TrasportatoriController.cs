using System;
using System.Web.Http;
using LatteMarche.Application.Trasportatori.Interfaces;
using LatteMarche.Application.Trasportatori.Dtos;
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

        #endregion

        #region Constructors

        public TrasportatoriController(ITrasportatoriService trasportatoriService)
		{
            this.trasportatoriService = trasportatoriService;
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
                return Ok(this.trasportatoriService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }
        
        #endregion


    }
}
