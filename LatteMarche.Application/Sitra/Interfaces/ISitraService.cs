using LatteMarche.Application.Latte.Dtos;
using System.Collections.Generic;

namespace LatteMarche.Application.Sitra.Interfaces
{
    public interface ISitraService
    {
        List<PrelievoLatteDto> InvioPrelievi(List<PrelievoLatteDto> prelievi);
        List<LottoDto> InvioLotti(List<LottoDto> lotti);
    }
}
