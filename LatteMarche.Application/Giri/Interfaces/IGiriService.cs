using System;
using LatteMarche.Application.Giri.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;

namespace LatteMarche.Application.Giri.Interfaces
{

    public interface IGiriService : IEntityService<Giro, int, GiroDto>
    {
        List<GiroDto> GetGiriOfTrasportatore(int idTrasportatore);

    }

}
