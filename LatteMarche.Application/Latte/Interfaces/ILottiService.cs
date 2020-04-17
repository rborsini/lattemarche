using LatteMarche.Application.Latte.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;

namespace LatteMarche.Application.Latte.Interfaces
{
    public interface ILottiService : IEntityService<Lotto, long, LottoDto>
    {
        /// <summary>
        /// Raggruppamento lotti
        /// </summary>
        /// <param name="prelievi"></param>
        /// <returns></returns>
        List<LottoDto> GetLotti(List<PrelievoLatteDto> prelievi);

        /// <summary>
        /// Ricerca per codice lotto
        /// </summary>
        /// <param name="codiceLotto"></param>
        /// <returns></returns>
        LottoDto GetByCodiceLotto(string codiceLotto);

    }
}
