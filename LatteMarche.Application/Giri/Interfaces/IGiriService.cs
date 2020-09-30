using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.Giri.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.Giri.Interfaces
{

    public interface IGiriService : IEntityService<Giro, int, GiroDto>
    {

        DropDownDto DropDownByTrasportatore(int idTrasportatore);


        DropDownDto DropDown(int? idUtente = (int?)null);

    }

}
