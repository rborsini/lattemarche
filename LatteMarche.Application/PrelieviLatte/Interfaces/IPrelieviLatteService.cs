using System;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;

namespace LatteMarche.Application.PrelieviLatte.Interfaces
{

    public interface IPrelieviLatteService : IEntityService<PrelievoLatte, int, PrelievoLatteDto>
    {

        /// <summary>
        /// Ricerca prelievi latte
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        List<PrelievoLatteDto> Search(PrelieviLatteSearchDto searchDto);

        /// <summary>
        /// Pull prelievi per sincronizzazione
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        List<PrelievoLatte> Pull(DateTime timestamp);

        /// <summary>
        /// Push prelievi per sincronizzazione
        /// </summary>
        /// <param name="list"></param>
        /// <returns>Nuovi prelievi inseriti</returns>
        List<PrelievoLatte> Push(List<PrelievoLatte> list);



    }

}
