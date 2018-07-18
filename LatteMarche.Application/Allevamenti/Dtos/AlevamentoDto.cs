using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Allevamenti.Dtos
{
    public class AllevamentoDto : EntityDto
    {
        public int Id { get; set; }
        public string CodiceAsl { get; set; }
        public string IndirizzoAllevamento { get; set; }
        public int IdUtente { get; set; }
        public int IdComune { get; set; }
        public int? IdSitraStabilimentoAllevamento { get; set; }
        public string CUAA { get; set; }
    }

    public class AllevamentiMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<Allevamento, AllevamentoDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.CodiceAsl, opts => opts.MapFrom(src => src.CodiceAsl.Trim()))
                .ForMember(dest => dest.IndirizzoAllevamento, opts => opts.MapFrom(src => src.IndirizzoAllevamento.Trim()))
                .ForMember(dest => dest.IdUtente, opts => opts.MapFrom(src => src.IdUtente))
                .ForMember(dest => dest.IdComune, opts => opts.MapFrom(src => src.IdComune))
                .ForMember(dest => dest.IdSitraStabilimentoAllevamento, opts => opts.MapFrom(src => src.IdSitraStabilimentoAllevamento))
                .ForMember(dest => dest.CUAA, opts => opts.MapFrom(src => src.CUAA))
                ;
            Mapper.CreateMap<AllevamentoDto, Allevamento>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.CodiceAsl, opts => opts.MapFrom(src => src.CodiceAsl.Trim()))
                .ForMember(dest => dest.IndirizzoAllevamento, opts => opts.MapFrom(src => src.IndirizzoAllevamento.Trim()))
                .ForMember(dest => dest.IdUtente, opts => opts.MapFrom(src => src.IdUtente))
                .ForMember(dest => dest.IdComune, opts => opts.MapFrom(src => src.IdComune))
                .ForMember(dest => dest.IdSitraStabilimentoAllevamento, opts => opts.MapFrom(src => src.IdSitraStabilimentoAllevamento))
                .ForMember(dest => dest.CUAA, opts => opts.MapFrom(src => src.CUAA))
                ;
        }
    }

}
