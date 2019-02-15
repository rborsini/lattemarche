using LatteMarche.Application.Destinatari.Dtos;
using LatteMarche.Application.LaboratoriAnalisi.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
