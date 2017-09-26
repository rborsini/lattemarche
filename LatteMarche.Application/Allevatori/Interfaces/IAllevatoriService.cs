using System;
using LatteMarche.Application.Allevatori.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;

namespace LatteMarche.Application.Allevatori.Interfaces
{

    public interface IAllevatoriService : IEntityReadOnlyService<V_Allevatore, int, AllevatoreDto>
    {

    }

}
