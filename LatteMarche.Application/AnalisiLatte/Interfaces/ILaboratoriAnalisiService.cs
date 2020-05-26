using LatteMarche.Core;
using LatteMarche.Core.Models;
using LatteMarche.Application.AnalisiLatte.Dtos;
using WeCode.Application.Interfaces;
using LatteMarche.Application.Common.Dtos;

namespace LatteMarche.Application.AnalisiLatte.Interfaces
{

    public interface ILaboratoriAnalisiService : IEntityService<LaboratorioAnalisi, int, LaboratorioAnalisiDto>
    {
        DropDownDto DropDown();
    }

}
