using LatteMarche.Application.AziendeTrasportatori.Dtos;
using LatteMarche.Application.Common.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.AziendeTrasportatori.Interfaces
{
    public interface IAziendeTrasportatoriService : IEntityService<AziendaTrasportatori, int, AziendaTrasportatoriDto>
    {
        DropDownDto DropDown(int? idUtente = (int?)null);
    }
}
