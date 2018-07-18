using LatteMarche.Application.Lotti.Dtos;
using LatteMarche.Application.PrelieviLatte.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Sitra.Interfaces
{
    public interface ISitraService
    {
        List<PrelievoLatteDto> InvioPrelievi(List<PrelievoLatteDto> prelievi);
        List<LottoDto> InvioLotti(List<LottoDto> lotti);
    }
}
