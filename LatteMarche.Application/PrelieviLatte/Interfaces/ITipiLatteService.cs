using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Core.Models;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.PrelieviLatte.Interfaces
{

    public interface ITipiLatteService : IEntityService<TipoLatte, int, TipoLatteDto>
    {
        DropDownDto DropDown();
    }

}
