using LatteMarche.Core.Models;
using WeCode.Application.Interfaces;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Common.Dtos;

namespace LatteMarche.Application.Utenti.Interfaces
{

    public interface ITipiProfiloService : IEntityReadOnlyService<TipoProfilo, int, TipoProfiloDto>
    {
        int GetIdProfilo(string DescrizioneProfilo);

        DropDownDto DropDown();
    }

}
