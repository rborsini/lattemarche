using LatteMarche.Application.Ruoli.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Ruoli.Interfaces
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
