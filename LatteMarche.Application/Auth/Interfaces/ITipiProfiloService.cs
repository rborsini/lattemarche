using LatteMarche.Core;
using LatteMarche.Core.Models;
using LatteMarche.Application.Auth.Dtos;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.Auth.Interfaces
{

    public interface ITipiProfiloService : IEntityReadOnlyService<TipoProfilo, int, TipoProfiloDto>
    {
        int GetIdProfilo(string DescrizioneProfilo);
    }

}
