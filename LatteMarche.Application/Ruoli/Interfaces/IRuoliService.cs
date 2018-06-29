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

        void RemoveUserInRole(long idRuolo, int username);

    }
}
