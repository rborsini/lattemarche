using System;
using LatteMarche.Application.Comuni.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using WeCode.Application.Interfaces;
using LatteMarche.Application.Common.Dtos;

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

        /// <summary>
        /// Elenco comuni appartenenti ad una provincia
        /// </summary>
        /// <param name="siglaProvincia"></param>
        /// <returns></returns>
        DropDownDto DropDown(string siglaProvincia);

    }

}
