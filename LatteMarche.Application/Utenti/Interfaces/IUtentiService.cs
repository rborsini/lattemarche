using System;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Utenti.Interfaces
{

    public interface IUtentiService : IEntityService<Utente, int, UtenteDto>
	{

		bool ValidateUser(string username, string password);

        //void SetPasswordHash(string username, string passwordHash);

        void SetToken(string username, string token);

        //void SetRole(string username, string role);

        string ChangePassword(string username, string oldPassword, string password, string rePassword);

        //string ResetPassword(string email);

        //string NewPassword(string resetPasswordId, string password, string rePassword);

        UtenteDto GetByUsername(string username);

    }

}
