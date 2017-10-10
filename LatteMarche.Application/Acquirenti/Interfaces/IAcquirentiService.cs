using System;
using LatteMarche.Application.Acquirenti.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Acquirenti.Interfaces
{

    public interface IAcquirentiService : IEntityService<Acquirente, int, AcquirenteDto>
	{

    }

}
