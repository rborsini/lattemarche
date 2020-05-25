using LatteMarche.Application.Cessionari.Dtos;
using LatteMarche.Application.Common.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.Cessionari.Interfaces
{

    public interface ICessionariService : IEntityService<Cessionario, int, CessionarioDto>
    {
        DropDownDto DropDown(int? idUtente = (int?)null);
    }
}
