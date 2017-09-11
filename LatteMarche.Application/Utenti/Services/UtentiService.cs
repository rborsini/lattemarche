using System;

using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using RB.Hash;

namespace LatteMarche.Application.Utenti.Services
{

    public class UtentiService : EntityService<Utente, int, UtenteDto>, IUtentiService
	{

		#region Fields

		private IRepository<Utente, int> utentiRepository;			

		#endregion

		#region Constructors

		public UtentiService(IUnitOfWork uow)
			: base(uow)
		{
			this.utentiRepository = this.uow.Get<Utente, int>();
		}

        #endregion

        #region Methods


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

                Utente utente = this.utentiRepository.FindBy(u => u.Username == username);

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

        public Utente GetByUsername(string username)
        {
            return this.utentiRepository.FindBy(u => u.Username == username);
        }

        /// <summary>
        /// Impostazione della password hashata
        /// </summary>
        /// <param name="username"></param>
        /// <param name="passwordHash"></param>
      /*  public void SetPasswordHash(string username, string passwordHash)
        {
            Utente utente = this.utentiRepository.GetByU(username);
            if (utente != null)
            {
                utente.Password = passwordHash;
                this.uow.SaveChanges();
            }
        }*/

        /// <summary>
        /// Validazione utente lato MVC
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ValidateUser(string username, string password)
        {
            string passwordHash = new HashHelper().HashPassword(password);

            Utente utente = this.utentiRepository.FindBy(u => u.Username == username && u.Password == passwordHash);
            return utente != null;
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

            return dbEntity;
		}

        public override UtenteDto Details(int key)
        {
            UtenteDto utenteDto = null;
            Utente utente = this.repository.GetById(key);
            if (utente != null)
            {
                utenteDto = ConvertToDto(utente);
            }

            return utenteDto;
        }

        #endregion
    }

}
