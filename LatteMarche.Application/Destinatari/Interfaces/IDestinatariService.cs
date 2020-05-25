using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.Destinatari.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.Destinatari.Interfaces
{
    public interface IDestinatariService : IEntityService<Destinatario, int, DestinatarioDto>
    {

        DropDownDto DropDown(int? idUtente = (int?)null);
    }
}
