using System;
using System.Web.Http;
using LatteMarche.WebApi.Attributes;
using WebApi.OutputCache.V2;
using RB.Hash;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Application.Auth.Dtos;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class UtentiController : ApiController
    {

        #region Fields

        private IUtentiService utentiService;
        private ITipiProfiloService tipiProfiloService;
        private IRuoliService ruoliService;

        #endregion

        #region Constructors

        public UtentiController(IUtentiService utentiService, ITipiProfiloService tipiProfiloService, IRuoliService ruoliService)
		{
            this.utentiService = utentiService;
            this.tipiProfiloService = tipiProfiloService;
		    this.ruoliService = ruoliService;
		}

        #endregion

        #region Methods

        [ViewItem(nameof(Index), "Utenti", "Lista")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            try
            {
                return Ok(this.utentiService.Index());
            }
            catch(Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Details), "Utenti", "Dettaglio")]
        [HttpGet]
        public IHttpActionResult Details(string username="", int id = 0)
        {
            try
            {
                if(!String.IsNullOrEmpty(username))
                    return Ok(this.utentiService.Details(username));
                else
                    return Ok(this.utentiService.Details(id));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }


        [ViewItem(nameof(Save), "Utenti", "Salvataggio")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] UtenteDto model)
        {
            try
            {
                if (model.Id == 0)
                {
                    model.Abilitato = true;
                    model.Password = new HashHelper().HashPassword(model.Password);

                    UtenteDto utente = this.utentiService.Create(model);

                    string tokenUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.LocalPath, "/Token");

                    return Ok(utente);
                }
                else
                {
                    // aggiornare ruoli/utenti
                    this.ruoliService.UpdateUserRole(model.Id, model.IdProfilo);

                    // aggiornare utente
                    var users = this.utentiService.Update(model);

                    return Ok(model);
                }
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }

        }

        [ViewItem(nameof(Delete), "Utenti", "Rimozione")]
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

        [ViewItem(nameof(Search), "Utenti", "Ricerca")]
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

        [ViewItem(nameof(Destinatari), "Utenti", "Destinatari")]
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
