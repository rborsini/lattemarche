using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.Autocisterne.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.Autocisterne.Interfaces
{
    public interface IAutocisterneService : IEntityService<Autocisterna, int, AutocisternaDto>
    {
        DropDownDto DropDown();
    }
}
