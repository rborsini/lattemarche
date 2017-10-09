using System;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;

namespace LatteMarche.Application.PrelieviLatte.Interfaces
{

    public interface IPrelieviLatteService : IEntityService<PrelievoLatte, int, PrelievoLatteDto>
    {
       
    }

}
