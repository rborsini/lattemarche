using LatteMarche.Application.AnalisiLatte.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.AnalisiLatte.Interfaces
{
    public interface IAnalisiService
    {
        /// <summary>
        /// Scarica le nuove analisi dall'ASSAM e le salva nel database locale
        /// </summary>
        void Synch();

        /// <summary>
        /// Ricerca analisi latte
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        List<AnalisiDto> Search(AnalisiSearchDto searchDto);

    }
}
