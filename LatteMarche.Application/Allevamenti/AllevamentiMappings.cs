using AutoMapper.Configuration;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Allevamenti
{
    public class AllevamentiMappings
    {
        internal static MapperConfigurationExpression Configure(MapperConfigurationExpression mappings)
        {
            // https://docs.automapper.org/en/stable/Reverse-Mapping-and-Unflattening.html
            mappings.CreateMap<Allevamento, AllevamentoDto>()
                .ReverseMap();

            mappings.CreateMap<V_Allevatore, AllevatoreDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.CodiceAsl, opts => opts.MapFrom(src => src.CodiceAsl.Trim()))
                .ForMember(dest => dest.IndirizzoAllevamento, opts => opts.MapFrom(src => src.IndirizzoAllevamento.Trim()))
                .ForMember(dest => dest.Nome, opts => opts.MapFrom(src => src.Nome.Trim()))
                .ForMember(dest => dest.Cognome, opts => opts.MapFrom(src => src.Cognome.Trim()))
                .ForMember(dest => dest.RagioneSociale, opts => opts.MapFrom(src => src.RagioneSociale.Trim()))
                .ForMember(dest => dest.Comune, opts => opts.MapFrom(src => src.Comune.Trim()))
                .ForMember(dest => dest.Provincia, opts => opts.MapFrom(src => src.Provincia.Trim()))
                .ForMember(dest => dest.IdUtente, opts => opts.MapFrom(src => src.IdUtente))
                .ForMember(dest => dest.IdComune, opts => opts.MapFrom(src => src.IdComune))
                .ForMember(dest => dest.IdSitraStabilimentoAllevamento, opts => opts.MapFrom(src => src.IdSitraStabilimentoAllevamento))
                ;

            mappings.CreateMap<AllevatoreDto, V_Allevatore>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.CodiceAsl, opts => opts.MapFrom(src => src.CodiceAsl.Trim()))
                .ForMember(dest => dest.IndirizzoAllevamento, opts => opts.MapFrom(src => src.IndirizzoAllevamento.Trim()))
                .ForMember(dest => dest.Nome, opts => opts.MapFrom(src => src.Nome.Trim()))
                .ForMember(dest => dest.Cognome, opts => opts.MapFrom(src => src.Cognome.Trim()))
                .ForMember(dest => dest.RagioneSociale, opts => opts.MapFrom(src => src.RagioneSociale.Trim()))
                .ForMember(dest => dest.Comune, opts => opts.MapFrom(src => src.Comune.Trim()))
                .ForMember(dest => dest.Provincia, opts => opts.MapFrom(src => src.Provincia.Trim()))
                .ForMember(dest => dest.IdUtente, opts => opts.MapFrom(src => src.IdUtente))
                .ForMember(dest => dest.IdComune, opts => opts.MapFrom(src => src.IdComune))
                .ForMember(dest => dest.IdSitraStabilimentoAllevamento, opts => opts.MapFrom(src => src.IdSitraStabilimentoAllevamento))
                ;

            return mappings;
        }
    }
}
