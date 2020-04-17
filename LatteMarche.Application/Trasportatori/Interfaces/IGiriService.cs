using LatteMarche.Application.Trasportatori.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;

namespace LatteMarche.Application.Trasportatori.Interfaces
{

    public interface IGiriService : IEntityService<Giro, int, GiroDto>
    {

        /// <summary>
        /// Recupero giri trasportatore
        /// </summary>
        /// <param name="idTrasportatore"></param>
        /// <returns></returns>
        List<GiroDto> GetGiriTrasportatore(int idTrasportatore);

    }

}
