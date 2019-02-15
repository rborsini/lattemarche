using System;
using LatteMarche.Application.Acquirenti.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Acquirenti.Interfaces
{

    public interface IAcquirentiService : IEntityService<Acquirente, int, AcquirenteDto>
	{
        /// <summary>
        /// Recupero dell'acquirente associato all'utente
        /// </summary>
        /// <param name="idUtente"></param>
        /// <returns></returns>
        AcquirenteDto GetByIdUtente(long idUtente);
    }

}
