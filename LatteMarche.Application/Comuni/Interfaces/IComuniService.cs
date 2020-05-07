using System;
using LatteMarche.Application.Comuni.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.Comuni.Interfaces
{

    public interface IComuniService : IEntityReadOnlyService<Comune, int, ComuneDto>
    {

        /// <summary>
        /// Ricerca comuni
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        List<ComuneDto> Search(ComuniSearchDto searchDto);

        /// <summary>
        /// Elenco province
        /// </summary>
        /// <returns></returns>
        List<string> GetProvince();

    }

}
