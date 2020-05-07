using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.Allevamenti.Interfaces
{

    public interface IAllevatoriService : IEntityReadOnlyService<V_Allevatore, int, AllevatoreDto>
    {

    }

}
