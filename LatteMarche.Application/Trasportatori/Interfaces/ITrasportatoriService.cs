using System;
using LatteMarche.Application.Trasportatori.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Trasportatori.Interfaces
{

    public interface ITrasportatoriService : IEntityReadOnlyService<V_Trasportatore, int, TrasportatoreDto>
	{
       

    }

}
