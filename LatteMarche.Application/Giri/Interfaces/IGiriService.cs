using System;
using LatteMarche.Application.Giri.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;

namespace LatteMarche.Application.Giri.Interfaces
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
