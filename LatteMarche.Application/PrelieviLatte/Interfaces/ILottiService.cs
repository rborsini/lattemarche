using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.PrelieviLatte.Interfaces
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
