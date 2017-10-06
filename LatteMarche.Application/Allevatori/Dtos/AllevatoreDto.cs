using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Allevatori.Dtos
{
    public class AllevatoreDto : EntityDto
    {
        public int Id { get; set; }
        public string CodiceAsl { get; set; }
        public string IndirizzoAllevamento { get; set; }
        public string Comune { get; set; }
        public string Provincia { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string RagioneSociale { get; set; }
        public int IdUtente { get; set; }
        public int IdComune { get; set; }
        public int? IdSitraStabilimentoAllevamento { get; set; }

    }

    public class AllevatoriMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<V_Allevatore, AllevatoreDto>()
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

            Mapper.CreateMap<AllevatoreDto, V_Allevatore  > ()
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
        }
    }

}
