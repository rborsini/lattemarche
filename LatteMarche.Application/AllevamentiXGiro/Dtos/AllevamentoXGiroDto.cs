using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.AllevamentiXGiro.Dtos
{
    public class AllevamentoXGiroDto : EntityDto
    {
        //public String Id { get; set; }
        public int IdGiro { get; set; }
        public int IdAllevamento { get; set; }
        /*public string Nome { get; set; }
        public string Cognome { get; set; }
        public string IndirizzoAllevamento { get; set; }
        public string Comune { get; set; }
        public string Provincia { get; set; }
        public string RagioneSociale { get; set; }*/
        public int? Priorita { get; set; }

    }

    public class AllevamentiXGiroMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<AllevamentoXGiro, AllevamentoXGiroDto>()
              ///  .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id.Trim()))
                .ForMember(dest => dest.IdGiro, opts => opts.MapFrom(src => src.IdGiro))
                .ForMember(dest => dest.IdAllevamento, opts => opts.MapFrom(src => src.IdAllevamento))
                .ForMember(dest => dest.Priorita, opts => opts.MapFrom(src => src.Priorita))
                ;

            Mapper.CreateMap<AllevamentoXGiroDto, AllevamentoXGiro  > ()
                ///  .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id.Trim()))
                .ForMember(dest => dest.IdGiro, opts => opts.MapFrom(src => src.IdGiro))
                .ForMember(dest => dest.IdAllevamento, opts => opts.MapFrom(src => src.IdAllevamento))
                .ForMember(dest => dest.Priorita, opts => opts.MapFrom(src => src.Priorita))
                ;
        }
    }

}
