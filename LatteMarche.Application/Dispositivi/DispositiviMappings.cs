using AutoMapper.Configuration;
using LatteMarche.Application.Dispositivi.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dispositivi
{
    public class DispositiviMappings
    {
        internal static MapperConfigurationExpression Configure(MapperConfigurationExpression mappings)
        {
            mappings.CreateMap<DispositivoMobile, DispositivoMobileDto>();

            mappings.CreateMap<DispositivoMobileDto, DispositivoMobile>();

            return mappings;
        }
    }
}
