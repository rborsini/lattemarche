using System;
using LatteMarche.Application.Comuni.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;

namespace LatteMarche.Application.Comuni.Interfaces
{

    public interface IComuniService : IEntityReadOnlyService<Comune, int, ComuneDto>
    {

        List<ComuneDto> Search(ComuniSearchDto searchDto);

        List<string> GetProvince();

    }

}
