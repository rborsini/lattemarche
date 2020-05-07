using LatteMarche.Application.Latte.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.Latte.Interfaces
{

    public interface ITipiLatteService : IEntityService<TipoLatte, int, TipoLatteDto>
    {

    }

}
