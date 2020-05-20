using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.Trasportatori.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.Trasportatori.Interfaces
{
    public interface IAutocisterneService : IEntityService<Autocisterna, int, AutocisternaDto>
    {
        DropDownDto DropDown(int? idTrasportatore);
    }
}
