using AutoMapper.Configuration;
using LatteMarche.Application.Destinatari.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Destinatari
{
    public class DestinatarioMappings
    {
        public static MapperConfigurationExpression Configure(MapperConfigurationExpression mappings)
        {
            mappings.CreateMap<Destinatario, DestinatarioDto>();
            mappings.CreateMap<DestinatarioDto, Destinatario>();

            return mappings;
        }
    }
}
