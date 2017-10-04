using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.AllevamentiXGiro.Dtos
{
    public class AllevamentoXGiroDto : EntityDto
    {
        public int Id { get; set; }
        public int IdGiro { get; set; }
        public int IdAllevamento { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string IndirizzoAllevamento { get; set; }
        public string Comune { get; set; }
        public string Provincia { get; set; }
        public string RagioneSociale { get; set; }
        public int? Priorita { get; set; }

    }

    public class AllevamentiXGiroMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<V_AllevamentoXGiro, AllevamentoXGiroDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.IdGiro, opts => opts.MapFrom(src => src.IdGiro))
                .ForMember(dest => dest.IdAllevamento, opts => opts.MapFrom(src => src.IdAllevamento))
                .ForMember(dest => dest.Nome, opts => opts.MapFrom(src => src.Nome.Trim()))
                .ForMember(dest => dest.Cognome, opts => opts.MapFrom(src => src.Cognome.Trim()))
                .ForMember(dest => dest.IndirizzoAllevamento, opts => opts.MapFrom(src => src.IndirizzoAllevamento.Trim()))
                .ForMember(dest => dest.Comune, opts => opts.MapFrom(src => src.Comune.Trim()))
                .ForMember(dest => dest.Provincia, opts => opts.MapFrom(src => src.Provincia.Trim()))
                .ForMember(dest => dest.RagioneSociale, opts => opts.MapFrom(src => src.RagioneSociale.Trim()))
                .ForMember(dest => dest.Priorita, opts => opts.MapFrom(src => src.Priorita))
                ;
        }
    }

}
