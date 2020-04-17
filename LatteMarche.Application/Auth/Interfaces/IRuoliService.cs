using LatteMarche.Application.Auth.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Auth.Interfaces
{
    public interface IRuoliService : IEntityService<Ruolo, long, RuoloDto>
    {

        /// <summary>
        /// Aggiornamento ruolo singolo utente
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        void UpdateUserRole(int userId, long role);

    }
}
