using System;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using RB.Hash;
using System.Collections.Generic;
using System.Linq;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Application.Comuni.Services;
using LatteMarche.Application.Comuni.Dtos;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
using WeCode.Data.Interfaces;
using WeCode.Application;
using LatteMarche.Application.Common.Dtos;
using Z.EntityFramework.Plus;

namespace LatteMarche.Application.Utenti.Services
{

    public class UtentiService : EntityService<Utente, int, UtenteDto>, IUtentiService
    {

        #region Fields

        private IRepository<Utente, int> utentiRepository;
        private IRepository<UtenteXAcquirente, int> utenteXAcquirenteRepository;
        private IRepository<UtenteXCessionario, int> utenteXCessionarioRepository;
        private IRepository<UtenteXDestinatario, int> utenteXDestinatarioRepository;
        private IRepository<Allevamento, int> allevamentiRepository;
        private IRepository<Autocisterna, int> autocisterneRepository;

        private IComuniService comuniService;

        #endregion

        #region Constructors

        public UtentiService(IUnitOfWork uow)
            : base(uow)
        {
            this.utentiRepository = this.uow.Get<Utente, int>();

            this.utenteXAcquirenteRepository = this.uow.Get<UtenteXAcquirente, int>();
            this.utenteXCessionarioRepository = this.uow.Get<UtenteXCessionario, int>();
            this.utenteXDestinatarioRepository = this.uow.Get<UtenteXDestinatario, int>();
            this.allevamentiRepository = this.uow.Get<Allevamento, int>();
            this.autocisterneRepository = this.uow.Get<Autocisterna, int>();

            this.comuniService = new ComuniService(uow);    // HACK: faccio la new perché IUtentiService è usato dal CustomUserStore
        }

        #endregion

        #region Methods

        public override void Delete(int key)
        {
            this.utenteXAcquirenteRepository.Delete(key);
            this.utenteXCessionarioRepository.Delete(key);
            this.utenteXDestinatarioRepository.Delete(key);

            var allevamentiDaRimuovere = this.allevamentiRepository.DbSet.Where(a => a.IdUtente == key).Select(a => a.Id).ToList();
            this.allevamentiRepository.Delete(allevamentiDaRimuovere.ToArray());

            var autocisterneDaRimuovere = this.autocisterneRepository.DbSet.Where(a => a.IdTrasportatore == key).Select(a => a.Id).ToList();
            this.autocisterneRepository.Delete(autocisterneDaRimuovere.ToArray());

            this.uow.SaveChanges();

            base.Delete(key);
        }

        /// <summary>
        /// Caricamento dettaglio utente tramite username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UtenteDto Details(string username)
        {
            UtenteDto utenteDto = null;

            if (String.IsNullOrEmpty(username))
                return utenteDto;

            Utente utente = this.utentiRepository.Query.FirstOrDefault(u => u.Username == username);

            if (utente != null)
            {
                utenteDto = ConvertToDto(utente);

                if (utenteDto.IdComune.HasValue)
                {
                    var comune = this.comuniService.Details(utenteDto.IdComune.Value);
                    if (comune != null)
                        utenteDto.SiglaProvincia = comune.Provincia;
                }

            }

            return utenteDto;
        }

        /// <summary>
        /// Caricamento dettaglio utente tramite id
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override UtenteDto Details(int key)
        {
            UtenteDto utenteDto = null;
            Utente utente = this.repository.GetById(key);
            if (utente != null)
            {
                utenteDto = ConvertToDto(utente);

                if (utenteDto.IdComune.HasValue)
                {
                    ComuneDto comuneDto = this.comuniService.Details(utenteDto.IdComune.Value);

                    if (comuneDto != null)
                        utenteDto.SiglaProvincia = comuneDto.Provincia;
                }
            }

            return utenteDto;
        }

        /// <summary>
        /// Validazione utente lato MVC
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ValidateUser(string username, string password)
        {
            string passwordHash = new HashHelper().HashPassword(password);

            Utente utente = this.utentiRepository.Query.FirstOrDefault(u => u.Username == username && u.Password == passwordHash);
            return utente != null;
        }

        /// <summary>
        /// Impostazione della password hashata
        /// </summary>
        /// <param name="username"></param>
        /// <param name="passwordHash"></param>
        public void SetPasswordHash(string username, string passwordHash)
        {
            Utente utente = this.utentiRepository.Query.FirstOrDefault(u => u.Username == username);
            if (utente != null)
            {
                utente.Password = passwordHash;
                this.uow.SaveChanges();
            }
        }

        /// <summary>
        /// Aggiornamento token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="token"></param>
        public void SetToken(string username, string token)
        {
            this.repository.DbSet
                .Where(u => u.Username == username)
                .Update(u => new Utente() { Token = token });
        }

        /// <summary>
        ///  Cambio password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="rePassword"></param>
        /// <returns></returns>
        public string ChangePassword(string username, string password, string rePassword)
        {
            string result = "";
            if (String.IsNullOrEmpty(password) || String.IsNullOrEmpty(rePassword))
            {
                result = "Password obbligatoria";
            }
            else
            {
                string newPasswordHash = new HashHelper().HashPassword(password);

                Utente utente = this.utentiRepository.Query.FirstOrDefault(u => u.Username == username);

                if (utente == null)
                {
                    result = "Nessun utente trovato";
                }
                else
                {
                    if (password != rePassword)
                    {
                        result = "Nuove password non corrispondenti";
                    }
                    else
                    {
                        utente.Password = newPasswordHash;
                        this.utentiRepository.Update(utente);
                        this.uow.SaveChanges();
                    }
                }
            }
            return result;
        }

        /// <summary>
        ///  Cambio password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldPassword"></param>
        /// <param name="password"></param>
        /// <param name="rePassword"></param>
        /// <returns></returns>
        public string ChangePassword(string username, string oldPassword, string password, string rePassword)
        {
            string result = "";
            if (String.IsNullOrEmpty(oldPassword) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(rePassword))
            {
                result = "Password obbligatoria";
            }
            else
            {
                string passwordHash = new HashHelper().HashPassword(oldPassword);
                string newPasswordHash = new HashHelper().HashPassword(password);

                Utente utente = this.utentiRepository.Query.FirstOrDefault(u => u.Username == username);

                if (utente == null)
                {
                    result = "Nessun utente trovato";
                }
                else
                {
                    if (utente.Password != passwordHash)
                    {
                        result = "Vecchia password errata";
                    }
                    else
                    {
                        if (password != rePassword)
                        {
                            result = "Nuove password non corrispondenti";
                        }
                        else
                        {
                            utente.Password = newPasswordHash;
                            this.utentiRepository.Update(utente);
                            this.uow.SaveChanges();
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Ricerca utenti
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        public List<UtenteDto> Search(UtentiSearchDto searchDto)
        {
            IQueryable<Utente> query = this.utentiRepository.Query;

            // Tipo profilo
            if (searchDto.IdProfilo != null)
            {
                query = query.Where(u => u.IdProfilo == searchDto.IdProfilo);
            }

            var list = query.ToList();

            return ConvertToDtoList(list);
        }


        protected override Utente UpdateProperties(Utente viewEntity, Utente dbEntity)
        {
            dbEntity.Nome = viewEntity.Nome;
            dbEntity.Cognome = viewEntity.Cognome;
            dbEntity.PivaCF = viewEntity.PivaCF;
            dbEntity.Indirizzo = viewEntity.Indirizzo;
            dbEntity.IdProfilo = viewEntity.IdProfilo;
            dbEntity.RagioneSociale = viewEntity.RagioneSociale;
            dbEntity.CodiceAllevatore = viewEntity.CodiceAllevatore;
            dbEntity.QuantitaLatte = viewEntity.QuantitaLatte;
            dbEntity.Telefono = viewEntity.Telefono;
            dbEntity.Cellulare = viewEntity.Cellulare;
            dbEntity.Sesso = viewEntity.Sesso;
            dbEntity.IdTipoLatte = viewEntity.IdTipoLatte;
            dbEntity.NumeroComunicazione = viewEntity.NumeroComunicazione;
            dbEntity.Note = viewEntity.Note;
            dbEntity.Abilitato = viewEntity.Abilitato;
            dbEntity.Visibile = viewEntity.Visibile;
            dbEntity.IdComune = viewEntity.IdComune;

            dbEntity.UtenteXAcquirente = UpdateUtenteXAcquirente(dbEntity.UtenteXAcquirente, viewEntity.UtenteXAcquirente);
            dbEntity.UtenteXCessionario = UpdateUtenteXCessionario(dbEntity.UtenteXCessionario, viewEntity.UtenteXCessionario);
            dbEntity.UtenteXDestinatario = UpdateUtenteXDestinatario(dbEntity.UtenteXDestinatario, viewEntity.UtenteXDestinatario);
            dbEntity.Allevamenti = UpdateAllevamenti(dbEntity.Allevamenti, viewEntity.Allevamenti);
            dbEntity.Autocisterne = UpdateAutocisterne(dbEntity.Autocisterne, viewEntity.Autocisterne);

            return dbEntity;
        }

        private List<Autocisterna> UpdateAutocisterne(List<Autocisterna> autocisterneDb, List<Autocisterna> autocisterneView)
        {
            // autocisterne rimosse
            var autocisterneDaRimuovere = new List<Autocisterna>();
            foreach (var autocisterna in autocisterneDb)
            {
                if (autocisterneView.FirstOrDefault(a => a.Id == autocisterna.Id) == null)
                    autocisterneDaRimuovere.Add(autocisterna);
            }

            foreach (var autocisterna in autocisterneDaRimuovere)
                this.autocisterneRepository.Delete(autocisterna);

            this.uow.SaveChanges();

            // autocisterne aggiunte e modificate
            foreach (var autocisternaView in autocisterneView)
            {
                var autocisernaDb = autocisterneDb.FirstOrDefault(a => a.Id == autocisternaView.Id);
                if (autocisernaDb == null)
                {
                    this.autocisterneRepository.Add(autocisternaView);
                    this.uow.SaveChanges();
                }
                else
                {
                    autocisernaDb = UpdateAutocisterneProperties(autocisernaDb, autocisternaView);
                    this.autocisterneRepository.Update(autocisernaDb);
                    this.uow.SaveChanges();
                }

            }

            return autocisterneView;
        }

        private Autocisterna UpdateAutocisterneProperties(Autocisterna autocisernaDb, Autocisterna autocisternaView)
        {
            autocisernaDb.Marca = autocisternaView.Marca;
            autocisernaDb.Modello = autocisternaView.Modello;
            autocisernaDb.IdTrasportatore = autocisternaView.IdTrasportatore;
            autocisernaDb.NumScomparti = autocisternaView.NumScomparti;
            autocisernaDb.Portata = autocisternaView.Portata;
            autocisernaDb.Targa = autocisternaView.Targa;

            return autocisernaDb;
        }

        private List<Allevamento> UpdateAllevamenti(List<Allevamento> allevamentiDb, List<Allevamento> allevamentiView)
        {
            // allevamenti rimossi
            var allevamentiDaRimuovere = new List<Allevamento>();
            foreach (var allevamento in allevamentiDb)
            {
                if (allevamentiView.FirstOrDefault(a => a.Id == allevamento.Id) == null)
                    allevamentiDaRimuovere.Add(allevamento);
            }

            foreach (var allevamento in allevamentiDaRimuovere)
                this.allevamentiRepository.Delete(allevamento);

            this.uow.SaveChanges();

            // allevamenti aggiunti e modificati
            foreach (var allevamentoView in allevamentiView)
            {
                var allevamentoDb = allevamentiDb.FirstOrDefault(a => a.Id == allevamentoView.Id);
                if (allevamentoDb == null)
                {
                    this.allevamentiRepository.Add(allevamentoView);
                    this.uow.SaveChanges();
                }
                else
                {
                    allevamentoDb = UpdateAllevamentiProperties(allevamentoDb, allevamentoView);
                    this.allevamentiRepository.Update(allevamentoDb);
                    this.uow.SaveChanges();
                }

            }

            return allevamentiView;
        }

        private Allevamento UpdateAllevamentiProperties(Allevamento allevamentoDb, Allevamento allevamentoView)
        {
            allevamentoDb.CodiceAsl = allevamentoView.CodiceAsl;
            allevamentoDb.IndirizzoAllevamento = allevamentoView.IndirizzoAllevamento;
            allevamentoDb.IdComune = allevamentoView.IdComune;
            allevamentoDb.CUAA = allevamentoView.CUAA;
            allevamentoDb.IdSitraStabilimentoAllevamento = allevamentoView.IdSitraStabilimentoAllevamento;
            allevamentoDb.Latitudine = allevamentoView.Latitudine;
            allevamentoDb.Longitudine = allevamentoView.Longitudine;

            return allevamentoDb;
        }

        private UtenteXDestinatario UpdateUtenteXDestinatario(UtenteXDestinatario dbEntity, UtenteXDestinatario viewEntity)
        {
            UtenteXDestinatario uxa = null;

            if (dbEntity == null && viewEntity != null)
            {
                uxa = new UtenteXDestinatario() { Id = viewEntity.Id, IdDestinatario = viewEntity.IdDestinatario };
                this.utenteXDestinatarioRepository.Add(uxa);
                this.uow.SaveChanges();
            }

            if (dbEntity != null && viewEntity != null)
            {
                dbEntity.IdDestinatario = viewEntity.IdDestinatario;
                uxa = dbEntity;
                this.utenteXDestinatarioRepository.Update(dbEntity);
                this.uow.SaveChanges();
            }

            return uxa;
        }

        private UtenteXCessionario UpdateUtenteXCessionario(UtenteXCessionario dbEntity, UtenteXCessionario viewEntity)
        {
            UtenteXCessionario uxa = null;

            if (dbEntity == null && viewEntity != null)
            {
                uxa = new UtenteXCessionario() { Id = viewEntity.Id, IdCessionario = viewEntity.IdCessionario };
                this.utenteXCessionarioRepository.Add(uxa);
                this.uow.SaveChanges();
            }

            if (dbEntity != null && viewEntity != null)
            {
                dbEntity.IdCessionario = viewEntity.IdCessionario;
                uxa = dbEntity;
                this.utenteXCessionarioRepository.Update(dbEntity);
                this.uow.SaveChanges();
            }

            return uxa;
        }

        private UtenteXAcquirente UpdateUtenteXAcquirente(UtenteXAcquirente dbEntity, UtenteXAcquirente viewEntity)
        {
            UtenteXAcquirente uxa = null;

            if (dbEntity == null && viewEntity != null)
            {
                uxa = new UtenteXAcquirente() { Id = viewEntity.Id, IdAcquirente = viewEntity.IdAcquirente };
                this.utenteXAcquirenteRepository.Add(uxa);
                this.uow.SaveChanges();
            }

            if (dbEntity != null && viewEntity != null)
            {
                dbEntity.IdAcquirente = viewEntity.IdAcquirente;
                uxa = dbEntity;
                this.utenteXAcquirenteRepository.Update(dbEntity);
                this.uow.SaveChanges();
            }

            return uxa;
        }

        public UtenteDto GetByUsername(string username)
        {
            Utente utente = this.utentiRepository.Query.FirstOrDefault(u => u.Username == username);

            return ConvertToDto(utente);
        }


        #endregion
    }

}
