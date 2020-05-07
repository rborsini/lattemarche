using LatteMarche.Core;
using LatteMarche.Core.Models;
using LatteMarche.Application.AnalisiLatte.Dtos;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.AnalisiLatte.Interfaces
{

    public interface ILaboratoriAnalisiService : IEntityReadOnlyService<LaboratorioAnalisi, int, LaboratorioAnalisiDto>
    {

    }

}
