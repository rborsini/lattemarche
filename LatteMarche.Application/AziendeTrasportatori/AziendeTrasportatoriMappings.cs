using AutoMapper.Configuration;
using LatteMarche.Application.AziendeTrasportatori.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.AziendeTrasportatori
{
    public class AziendeTrasportatoriMappings
    {
        public static MapperConfigurationExpression Configure(MapperConfigurationExpression mappings)
        {
            mappings.CreateMap<AziendaTrasportatori, AziendaTrasportatoriDto>()
                ;

            mappings.CreateMap<AziendaTrasportatoriDto, AziendaTrasportatori>()
                ;

            return mappings;
        }
    }
}
