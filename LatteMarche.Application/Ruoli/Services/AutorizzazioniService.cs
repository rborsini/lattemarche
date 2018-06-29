using LatteMarche.Application.Ruoli.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace LatteMarche.Application.Ruoli.Services
{
    public class AutorizzazioniService : IAutorizzazioniService
    {
        #region Fields

        private IRepository<Utente, int> utentiRepository;
        private IRepository<Autorizzazione, long> autorizzazioniRepository;
        private IRepository<Azione, string> azioniRepository;

        #endregion

        #region Constructors

        public AutorizzazioniService(IUnitOfWork uow)
        {
            this.utentiRepository = uow.Get<Utente, int>();
            this.autorizzazioniRepository = uow.Get<Autorizzazione, long>();
            this.azioniRepository = uow.Get<Azione, string>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Autorizzazione invocazione action
        /// </summary>
        /// <param name="username"></param>
        /// <param name="type"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool Authorize(HttpSessionStateBase session, string username, string type, string controllerName, string actionName)
        {
            InitSession(session, username);
            List<AutorizzazioneSessione> autorizzazioniSessione = ParseSession(session);

            AutorizzazioneSessione autorizzazione = autorizzazioniSessione
                .FirstOrDefault(a => a.Type == type &&
                                        a.Controller == controllerName &&
                                        a.Action == actionName &&
                                        a.Authorized
                );

            return autorizzazione != null;
        }

        /// <summary>
        /// Autorizzazione invocazione action
        /// </summary>
        /// <param name="username"></param>
        /// <param name="type"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool Authorize(HttpSessionState session, string username, string type, string controllerName, string actionName)
        {
            InitSession(session, username);
            List<AutorizzazioneSessione> autorizzazioniSessione = ParseSession(session);

            AutorizzazioneSessione autorizzazione = autorizzazioniSessione
                .FirstOrDefault(a => a.Type == type &&
                                        a.Controller == controllerName &&
                                        a.Action == actionName &&
                                        a.ViewItem == actionName
                );

            return autorizzazione != null && autorizzazione.Authorized;
        }

        /// <summary>
        /// Autorizzazione viewItem
        /// </summary>
        /// <param name="username"></param>
        /// <param name="type"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool Authorize(HttpSessionState session, string username, string type, string controllerName, string actionName, string viewItem)
        {
            //bool authorized = false;

            InitSession(session, username);
            List<AutorizzazioneSessione> autorizzazioniSessione = ParseSession(session);

            AutorizzazioneSessione autorizzazione = autorizzazioniSessione
                .FirstOrDefault(a => a.Type == type &&
                                        a.Controller == controllerName &&
                                        a.Action == actionName &&
                                        a.ViewItem == viewItem
                );

            return autorizzazione != null && autorizzazione.Authorized;
        }

        /// <summary>
        /// Generazione dei tokens di abilitazione dei viewItems relativi all'action passata come parametro
        /// </summary>
        /// <param name="username"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public Dictionary<string, bool> GetViewBagTokens(HttpSessionState session, string username, string controllerName, string actionName)
        {
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
            //List<string> authorizedViewItems = new List<string>();

            // inizializzazione sessione
            InitSession(session, username);
            List<AutorizzazioneSessione> autorizzazioniSessione = ParseSession(session);

            autorizzazioniSessione = autorizzazioniSessione
                .Where(a => a.Type == "MVC" &&
                            a.Controller == controllerName &&
                            a.Action == actionName &&
                            a.Action != a.ViewItem)
                .ToList();

            // generazione dictionary
            foreach (AutorizzazioneSessione autorizzazione in autorizzazioniSessione)
            {
                dictionary.Add(autorizzazione.ViewItem, autorizzazione.Authorized);
            }

            return dictionary;
        }

        /// <summary>
        /// Controlla se la sessione contiene già il dictionary delle autorizzazioni ed eventualmente la riempe tramite i valori presi da DB
        /// </summary>
        /// <param name="session"></param>
        /// <param name="username"></param>
        private void InitSession(HttpSessionState session, string username)
        {
            if (session["username"] == null || session["username"].ToString() != username)
            {
                Dictionary<string, string> autorizzazioniDb = GetAutorizzazioniFromDb(username);

                foreach (string key in autorizzazioniDb.Keys)
                {
                    session[key] = autorizzazioniDb[key];
                }

                session["username"] = username;
            }
        }

        /// <summary>
        /// Controlla se la sessione contiene già il dictionary delle autorizzazioni ed eventualmente la riempe tramite i valori presi da DB
        /// </summary>
        /// <param name="session"></param>
        /// <param name="username"></param>
        private void InitSession(HttpSessionStateBase session, string username)
        {
            if (session["username"] == null || session["username"].ToString() != username)
            {
                Dictionary<string, string> autorizzazioniDb = GetAutorizzazioniFromDb(username);

                foreach (string key in autorizzazioniDb.Keys)
                {
                    session[key] = autorizzazioniDb[key];
                }

                session["username"] = username;
            }
        }

        /// <summary>
        /// Ritorna un dictionary con chiave IdAzione e valore la concatenazione dei ruoli abilitati
        /// Prende i dati dal database
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetAutorizzazioniFromDb(string username)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (!String.IsNullOrEmpty(username))
            {
                List<Azione> azioni = azioniRepository.GetAll().ToList();
                List<Autorizzazione> autorizzazioni = autorizzazioniRepository.GetAll().ToList();
                List<long> ruoli = utentiRepository
                    .FindBy(u => u.Username == username)
                    .RuoliUtente
                    .Where(r => r.IdRuolo.HasValue)
                    .Select(r => r.IdRuolo.Value)
                    .ToList();

                foreach (Azione azione in azioni)
                {
                    List<long> ruoliAutorizzati = autorizzazioni
                        .Where(a => a.Azione == azione.Id && ruoli.Contains(a.IdRuolo))
                        .Select(a => a.IdRuolo)
                        .ToList();

                    result.Add(azione.Id + "|" + azione.ViewItem, String.Join("|", ruoliAutorizzati));
                }
            }

            return result;
        }

        /// <summary>
        /// Recupero della struttura delle autorizzazioni dalla sessione
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private List<AutorizzazioneSessione> ParseSession(HttpSessionState session)
        {
            List<AutorizzazioneSessione> autorizzazioni = new List<AutorizzazioneSessione>();

            foreach (string key in session.Keys)
            {
                if (key.StartsWith("MVC") || key.StartsWith("API"))
                {
                    autorizzazioni.Add(new AutorizzazioneSessione(key, session[key].ToString()));
                }
            }

            return autorizzazioni;
        }

        /// <summary>
        /// Recupero della struttura delle autorizzazioni dalla sessione
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private List<AutorizzazioneSessione> ParseSession(HttpSessionStateBase session)
        {
            List<AutorizzazioneSessione> autorizzazioni = new List<AutorizzazioneSessione>();

            foreach (string key in session.Keys)
            {
                if (key.StartsWith("MVC") || key.StartsWith("API"))
                {
                    autorizzazioni.Add(new AutorizzazioneSessione(key, session[key].ToString()));
                }
            }

            return autorizzazioni;
        }



        #endregion

        private class AutorizzazioneSessione
        {
            public string Id { get; private set; }
            public string Type { get; set; }
            public string Controller { get; private set; }
            public string Action { get; private set; }
            public string ViewItem { get; private set; }
            public List<long> Ruoli { get; private set; }

            public bool Authorized
            {
                get { return this.Ruoli.Count > 0; }
            }

            public AutorizzazioneSessione(string key, string value)
            {
                string[] keyParts = key.Split('|');

                this.Id = keyParts[0];
                this.ViewItem = keyParts[1];

                string[] idParts = this.Id.Split('-');
                this.Type = idParts[0];
                this.Controller = idParts[1];
                this.Action = idParts[2];

                List<string> ruoliStr = value.Split('|').ToList();
                this.Ruoli = ruoliStr
                    .Where(r => !String.IsNullOrEmpty(r))
                    .Select(r => Convert.ToInt64(r))
                    .Distinct()
                    .ToList();
            }

        }

    }
}
