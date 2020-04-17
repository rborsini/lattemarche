using System;
using System.Web.Http;
using LatteMarche.Application.Trasportatori.Interfaces;
using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    //[ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
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

        [ViewItem(nameof(Index), "Trasportatori", "Lista")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            try
            {
                return Ok(this.trasportatoriService.Index());
            }
            catch(Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Details), "Trasportatori", "Dettaglio")]
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
