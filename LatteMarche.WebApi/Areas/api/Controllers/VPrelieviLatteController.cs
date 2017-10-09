using System;
using System.Web.Http;
using LatteMarche.Application.VPrelieviLatte.Interfaces;
using LatteMarche.Application.VPrelieviLatte.Dtos;
using Newtonsoft.Json.Linq;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class VPrelieviLatteController : ApiController
    {

        #region Fields

        private IVPrelieviLatteService vPrelieviLatteService;      

        #endregion

        #region Constructors

        public VPrelieviLatteController(IVPrelieviLatteService vPrelieviLatteService)
		{
            this.vPrelieviLatteService = vPrelieviLatteService;     
        }

        #endregion

        #region Methods

        [HttpGet]
        [HttpPost]
        public IHttpActionResult Index()
        {
            try
            {
                return Ok(this.vPrelieviLatteService.Index());
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
                return Ok(this.vPrelieviLatteService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpGet]
        [HttpPost]
        public IHttpActionResult Index(int idAllevamento)
        {
            try
            {
                return Ok(this.vPrelieviLatteService.getPrelieviByIdAllevamento(idAllevamento));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        #endregion


    }
}
