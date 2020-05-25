using System;
using LatteMarche.Application.Acquirenti.Dtos;
using LatteMarche.Application.Common.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.Acquirenti.Interfaces
{

    public interface IAcquirentiService : IEntityService<Acquirente, int, AcquirenteDto>
	{
        DropDownDto DropDown(int? idUtente = (int?)null);
    }

}
