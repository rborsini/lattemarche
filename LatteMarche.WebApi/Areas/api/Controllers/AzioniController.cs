using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.WebApi.Attributes;
using LatteMarche.WebApi.Filters;
using LatteMarche.WebApi.Helpers;
using System.Web.Http;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class AzioniController : ApiController
    {
        private IAzioniService azioniService;

        public AzioniController(IAzioniService service)
        {
            this.azioniService = service;
        }

        [ViewItem("Index", "Azioni", "Lista")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            return Ok(this.azioniService.Index());
        }

        [ViewItem("Details", "Azioni", "Dettaglio")]
        [HttpGet]
        public IHttpActionResult Details(string id)
        {
            return Ok(this.azioniService.Details(id));
        }


        [ViewItem("Synch", "Azioni", "Sincronizzazione azioni")]
        [HttpPost]
        public void Synch()
        {
            this.azioniService.Synch(ReflectionHelper.GetAzioni());
        }

    }
}