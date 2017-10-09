using System;
using LatteMarche.Application.VPrelieviLatte.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;

namespace LatteMarche.Application.VPrelieviLatte.Interfaces
{

    public interface IVPrelieviLatteService : IEntityReadOnlyService<V_PrelievoLatte, int, VPrelievoLatteDto>
    {
        List<VPrelievoLatteDto> getPrelieviByIdAllevamento(int idAllevamento);
    }

}
