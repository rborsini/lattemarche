using System;
using System.Web.Http;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.TipiProfilo.Interfaces;
using LatteMarche.Application.TipiProfilo.Dtos;
using Newtonsoft.Json.Linq;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;
using RB.Hash;

namespace LatteMarche.WebApi.Areas.api.Controllers
{
    [ApiCustomAuthorize]
    public class UtentiController : ApiController
    {

        #region Fields

        private IUtentiService utentiService;
        private ITipiProfiloService tipiProfiloService;

        #endregion

        #region Constructors

        public UtentiController(IUtentiService utentiService, ITipiProfiloService tipiProfiloService)
		{
            this.utentiService = utentiService;
            this.tipiProfiloService = tipiProfiloService;
        }

        #endregion

        #region Methods

        [HttpGet]
        [HttpPost]
        public IHttpActionResult Index()
        {
            try
            {


                var users = this.utentiService.Index();

                //DataTableResult<UtenteDto> result = new DataTableResult<UtenteDto>();

                //result.meta.page = 1;
                //result.meta.pages = users.Count / 10;
                //result.meta.perpage = 10;
                //result.meta.total = users.Count;
                //result.meta.sort = "asc";
                //result.meta.field = "Nome";

                //result.data = users;

                //return Ok(result);                

                return Ok(users);
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
                return Ok(this.utentiService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpPut]
        public IHttpActionResult Update([FromBody] UtenteDto model)
        {
            try
            {
                var users = this.utentiService.Update(model);
                return Ok(model);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] UtenteDto model)
        {
            try
            {
                model.Abilitato = true;
                model.Password = new HashHelper().HashPassword(model.Password);

                UtenteDto utente = this.utentiService.Create(model);

                string tokenUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.LocalPath, "/Token");

                return Ok(utente);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                this.utentiService.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public IHttpActionResult Search(int idProfilo)
        {
            //possibilità di mettere altri parametri per la search
            try
            {
                return Ok(this.utentiService.Search(new UtentiSearchDto() { IdProfilo = idProfilo }));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public IHttpActionResult Destinatari()
        {
            try
            {
                int idProfilo = tipiProfiloService.GetIdProfilo("Destinatario");
                return Ok(this.utentiService.Search(new UtentiSearchDto() { IdProfilo = idProfilo }));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        #endregion


    }
}
