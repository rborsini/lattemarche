using AutoMapper.Configuration;
using LatteMarche.Application.Dashboard.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dashboard
{
    public class WidgetsMappings
    {
        internal static MapperConfigurationExpression Configure(MapperConfigurationExpression mappings)
        {
            mappings.CreateMap<V_PrelievoLatte, WidgetAnalisiQuantitativaDto.Record>()
                .ForMember(dest => dest.Data, opts => opts.MapFrom(src => src.DataPrelievo))
                .ForMember(dest => dest.Qta_Kg, opts => opts.MapFrom(src => src.Quantita))
                .ForMember(dest => dest.Qta_Lt, opts => opts.MapFrom(src => src.QuantitaLitri))
                .ForMember(dest => dest.Temperatura, opts => opts.MapFrom(src => src.Temperatura))
                .ForMember(dest => dest.Trasportatore, opts => opts.MapFrom(src => src.Trasportatore))
                .ForMember(dest => dest.Acquirente, opts => opts.MapFrom(src => src.Acquirente))
                .ForMember(dest => dest.Destinatario, opts => opts.MapFrom(src => src.Destinatario))
                ;

            return mappings;
        }
    }
}
