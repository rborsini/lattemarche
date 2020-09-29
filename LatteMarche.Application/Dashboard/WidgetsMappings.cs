using AutoMapper.Configuration;
using LatteMarche.Application.AnalisiLatte.Dtos;
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

            mappings.CreateMap<AnalisiDto, WidgetAnalisiQualitativaDto.Record>()
                .ForMember(dest => dest.Campione, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.CodiceASL, opts => opts.MapFrom(src => src.CodiceASL))
                .ForMember(dest => dest.DataRapporto, opts => opts.MapFrom(src => src.DataRapportoDiProva))
                .ForMember(dest => dest.DataAccettazione, opts => opts.MapFrom(src => src.DataAccettazione))
                .ForMember(dest => dest.DataPrelievo, opts => opts.MapFrom(src => src.DataPrelievo))
                .ForMember(dest => dest.Grasso, opts => opts.MapFrom(src => src.Grasso))
                .ForMember(dest => dest.Proteine, opts => opts.MapFrom(src => src.Proteine))
                .ForMember(dest => dest.CaricaBatterica, opts => opts.MapFrom(src => src.CaricaBatterica))
                .ForMember(dest => dest.CelluleSomatiche, opts => opts.MapFrom(src => src.CelluleSomatiche))
                ;

            return mappings;
        }
    }
}
