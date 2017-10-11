using System;
using LatteMarche.Application.TipiProfilo.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;

namespace LatteMarche.Application.TipiProfilo.Interfaces
{

    public interface ITipiProfiloService : IEntityReadOnlyService<TipoProfilo, int, TipoProfiloDto>
    {
        int getIdProfilo(string DescrizioneProfilo);
    }

}
