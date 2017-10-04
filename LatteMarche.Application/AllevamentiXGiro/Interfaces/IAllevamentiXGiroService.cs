using System;
using LatteMarche.Application.AllevamentiXGiro.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;

namespace LatteMarche.Application.AllevamentiXGiro.Interfaces
{

    public interface IAllevamentiXGiroService : IEntityReadOnlyService<V_AllevamentoXGiro, int, AllevamentoXGiroDto>
    {
        List<AllevamentoXGiroDto> GetByGiro(int idGiro);
    }

}
