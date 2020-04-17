using LatteMarche.Application.Auth.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;

namespace LatteMarche.Application.Auth.Interfaces
{
    public interface IAzioniService : IEntityService<Azione, string, AzioneDto>
    {
        /// <summary>
        /// Aggiornamento del database con tutte le Action presenti nel database
        /// </summary>
        /// <param name="azioniReflection"></param>
        void Synch(List<AzioneDto> azioniReflection);

    }
}
