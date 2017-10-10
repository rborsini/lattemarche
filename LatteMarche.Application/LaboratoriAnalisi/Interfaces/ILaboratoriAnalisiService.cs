using System;
using LatteMarche.Application.LaboratoriAnalisi.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;

namespace LatteMarche.Application.LaboratoriAnalisi.Interfaces
{

    public interface ILaboratoriAnalisiService : IEntityReadOnlyService<LaboratorioAnalisi, int, LaboratorioAnalisiDto>
    {

    }

}
