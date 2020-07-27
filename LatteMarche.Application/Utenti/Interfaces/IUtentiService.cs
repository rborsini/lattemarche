using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using LatteMarche.Application.Utenti.Dtos;
using WeCode.Application.Interfaces;
using LatteMarche.Application.Common.Dtos;
using WeCode.Application;

namespace LatteMarche.Application.Utenti.Interfaces
{

    public interface IUtentiService : IEntityService<Utente, int, UtenteDto>
	{

        /// <summary>
        /// Recupero utente tramite username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        UtenteDto Details(string username);

        /// <summary>
        /// Validazione utente lato MVC
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
		bool ValidateUser(string username, string password);

        /// <summary>
        /// Aggiornamento del campo PasswordHash
        /// </summary>
        /// <param name="username"></param>
        /// <param name="passwordHash"></param>
        void SetPasswordHash(string username, string passwordHash);

        /// <summary>
        /// Salvataggio del token di accesso alle API
        /// </summary>
        /// <param name="username"></param>
        /// <param name="token"></param>
        void SetToken(string username, string token);

        /// <summary>
        /// Cambio password (per utenti base)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldPassword"></param>
        /// <param name="password"></param>
        /// <param name="rePassword"></param>
        /// <returns></returns>
        string ChangePassword(string username, string oldPassword, string password, string rePassword);

        /// <summary>
        /// Cambio password (per utenti amministratori)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="rePassword"></param>
        /// <returns></returns>
        string ChangePassword(string username, string password, string rePassword);

        /// <summary>
        /// Ricerca utenti
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        PagedResult<UtenteDto> Search(UtentiSearchDto searchDto);

        /// <summary>
        /// Restituisce l'utente a partire dall'username
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        UtenteDto GetByUsername(string name);
    }

}
