using LatteMarche.Application.Ruoli.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Ruoli.Interfaces
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
