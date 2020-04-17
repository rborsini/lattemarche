using AutoMapper.Configuration;
using LatteMarche.Application.AnalisiLatte.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.AnalisiLatte
{
    public class AnalisiMappings
    {
        public static MapperConfigurationExpression Configure(MapperConfigurationExpression mappings)
        {
            mappings.CreateMap<Assam.Models.Misura, ValoreAnalisiDto>();

            mappings.CreateMap<Assam.Models.AnalisiLatte, AnalisiDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Campione.Trim()))
                ;

            mappings.CreateMap<Analisi, AnalisiDto>();
            mappings.CreateMap<ValoreAnalisi, ValoreAnalisiDto>();

            mappings.CreateMap<AnalisiDto, Analisi>();
            mappings.CreateMap<ValoreAnalisiDto, ValoreAnalisi>();

            mappings.CreateMap<LaboratorioAnalisi, LaboratorioAnalisiDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Descrizione, opts => opts.MapFrom(src => src.Descrizione.Trim()))
                ;

            return mappings;

        }
    }
}
