using LatteMarche.Application.Lotti.Dtos;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Lotti.Interfaces
{
    public interface ILottiService : IEntityService<Lotto, long, LottoDto>
    {
        /// <summary>
        /// Raggruppamento lotti
        /// </summary>
        /// <param name="prelievi"></param>
        /// <returns></returns>
        List<LottoDto> GetLotti(List<PrelievoLatte> prelievi);

        /// <summary>
        /// Ricerca per codice lotto
        /// </summary>
        /// <param name="codiceLotto"></param>
        /// <returns></returns>
        LottoDto GetByCodiceLotto(string codiceLotto);

    }
}
