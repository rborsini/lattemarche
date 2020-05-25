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

namespace LatteMarche.Application.Utenti.Services
{

    public class UtentiService : EntityService<Utente, int, UtenteDto>, IUtentiService
	{

		#region Fields

		private IRepository<Utente, int> utentiRepository;
        private IRepository<UtenteXAcquirente, int> utenteXAcquirenteRepository;
        private IRepository<UtenteXCessionario, int> utenteXCessionarioRepository;
        private IRepository<UtenteXDestinatario, int> utenteXDestinatarioRepository;
        private IRepository<TrasportatoreXAzienda, int> trasportatoriXAziendeRepository;

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
            this.trasportatoriXAziendeRepository = this.uow.Get<TrasportatoreXAzienda, int>();

            this.comuniService = new ComuniService(uow);    // HACK: faccio la new perché IUtentiService è usato dal CustomUserStore
		}

        #endregion

        #region Methods

        public override void Delete(int key)
        {
            this.utenteXAcquirenteRepository.Delete(key);
            this.utenteXCessionarioRepository.Delete(key);
            this.utenteXDestinatarioRepository.Delete(key);
            this.trasportatoriXAziendeRepository.Delete(key);
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

            if(utente != null)
            {
                utenteDto = ConvertToDto(utente);

                if(utenteDto.IdComune.HasValue)
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

                if(utenteDto.IdComune.HasValue)
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

            return ConvertToDtoList(query.ToList());
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
            dbEntity.TrasportatoreXAzienda = UpdateTrasportatoreXAzienda(dbEntity.TrasportatoreXAzienda, viewEntity.TrasportatoreXAzienda);

            return dbEntity;
        }

        private TrasportatoreXAzienda UpdateTrasportatoreXAzienda(TrasportatoreXAzienda dbEntity, TrasportatoreXAzienda viewEntity)
        {
            TrasportatoreXAzienda uxa = null;

            if (dbEntity == null && viewEntity != null)
            {
                uxa = new TrasportatoreXAzienda() { Id = viewEntity.Id, IdAzienda = viewEntity.IdAzienda };
                this.trasportatoriXAziendeRepository.Add(uxa);
                this.uow.SaveChanges();
            }

            if (dbEntity != null && viewEntity != null)
            {
                dbEntity.IdAzienda = viewEntity.IdAzienda;
                this.trasportatoriXAziendeRepository.Update(dbEntity);
                this.uow.SaveChanges();
            }

            return uxa;
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

            if(dbEntity != null && viewEntity != null)
            {
                dbEntity.IdAcquirente = viewEntity.IdAcquirente;
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
