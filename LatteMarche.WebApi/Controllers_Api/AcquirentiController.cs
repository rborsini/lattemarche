using System;
using System.Web.Http;
using LatteMarche.Application.Acquirenti.Interfaces;
using LatteMarche.Application.Acquirenti.Dtos;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.Utenti.Interfaces;
using WeCode.MVC.Attributes;
using System.Threading;
using System.Threading.Tasks;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class AcquirentiController : ApiController
    {

        #region Fields

        private IAcquirentiService acquirentiService;
        private IUtentiService utentiService;

        #endregion

        #region Constructors

        public AcquirentiController(IAcquirentiService acquirentiService, IUtentiService utentiService)
        {
            this.acquirentiService = acquirentiService;
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Acquirenti", "Lista")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Index()
        {
            var acquirenti = this.acquirentiService.Index();
            return Ok(acquirenti);
        }

        [ViewItem(nameof(Details), "Acquirenti", "Dettaglio")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Details(int id)
        {
            return Ok(this.acquirentiService.Details(id));
        }

        [ViewItem(nameof(Dropdown), "Acquirenti", "Dropdown")]
        [HttpGet]
        [ETag]
        public async Task<IHttpActionResult> Dropdown()
        {

            Thread.Sleep(3000);

            var utente = this.utentiService.Details(User.Identity.Name);
            var dropDown = this.acquirentiService.DropDown(utente.Id);

            return Ok(Task.FromResult(dropDown));

        }

        [ViewItem(nameof(Save), "Acquirenti", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] AcquirenteDto model)
        {

            if (model.Id == 0)
                return Ok(this.acquirentiService.Create(model));
            else
                return Ok(this.acquirentiService.Update(model));

        }

        [ViewItem(nameof(Delete), "Acquirenti", "Rimozione")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {

            this.acquirentiService.Delete(id);
            return Ok();

        }

        #endregion


    }
}
