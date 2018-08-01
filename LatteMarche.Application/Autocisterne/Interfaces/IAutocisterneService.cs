using LatteMarche.Application.Autocisterne.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Autocisterne.Interfaces
{
    public interface IAutocisterneService : IEntityService<Autocisterna, int, AutocisternaDto>
    {
    }
}
