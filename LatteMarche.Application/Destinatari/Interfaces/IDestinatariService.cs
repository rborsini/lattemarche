using LatteMarche.Application.Destinatari.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Destinatari.Interfaces
{
    public interface IDestinatariService : IEntityService<Destinatario, int, DestinatarioDto>
    {
        /// <summary>
        /// Recupero del destinatario associato all'utente
        /// </summary>
        /// <param name="idUtente"></param>
        /// <returns></returns>
        DestinatarioDto GetByIdUtente(long idUtente);
    }
}
