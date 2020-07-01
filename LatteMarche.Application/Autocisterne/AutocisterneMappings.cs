using AutoMapper.Configuration;
using LatteMarche.Application.Autocisterne.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Trasportatori
{


    public class AutocisterneMappings
    {
        public static MapperConfigurationExpression Configure(MapperConfigurationExpression mappings)
        {
            mappings.CreateMap<Autocisterna, AutocisternaDto>();
            mappings.CreateMap<AutocisternaDto, Autocisterna>()
                .ForMember(dest => dest.Abilitato, opts => opts.MapFrom(src => true))
                ;

            return mappings;
        }
    }

}
