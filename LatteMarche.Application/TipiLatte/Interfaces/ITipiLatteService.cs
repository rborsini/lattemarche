using System;
using LatteMarche.Application.TipiLatte.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;

namespace LatteMarche.Application.TipiLatte.Interfaces
{

    public interface ITipiLatteService : IEntityReadOnlyService<TipoLatte, int, TipoLatteDto>
    {

       // List<TipoLatteDto> Search(int id);

    }

}
