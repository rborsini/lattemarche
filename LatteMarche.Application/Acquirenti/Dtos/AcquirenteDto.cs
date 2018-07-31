using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Acquirenti.Dtos
{
    public class AcquirenteDto : EntityDto
    {
        public int Id { get; set; }
        public string RagioneSociale { get; set; }
        public string Piva { get; set; }
        public string Indirizzo { get; set; }
        public string SiglaProvincia { get; set; }
        public int IdComune { get; set; }
        public int? IdSitra { get; set; }
    }

    public class AcquirentiMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<Acquirente, AcquirenteDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.RagioneSociale, opts => opts.MapFrom(src => src.RagioneSociale.Trim()))
                .ForMember(dest => dest.Piva, opts => opts.MapFrom(src => src.Piva.Trim()))
                .ForMember(dest => dest.Indirizzo, opts => opts.MapFrom(src => src.Indirizzo.Trim()))
                .ForMember(dest => dest.IdComune, opts => opts.MapFrom(src => src.IdComune))
                .ForMember(dest => dest.IdSitra, opts => opts.MapFrom(src => src.IdSitra))
                ;
            Mapper.CreateMap<AcquirenteDto, Acquirente>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.RagioneSociale, opts => opts.MapFrom(src => src.RagioneSociale.Trim()))
                .ForMember(dest => dest.Piva, opts => opts.MapFrom(src => src.Piva.Trim()))
                .ForMember(dest => dest.Indirizzo, opts => opts.MapFrom(src => src.Indirizzo.Trim()))
                .ForMember(dest => dest.IdComune, opts => opts.MapFrom(src => src.IdComune))
                .ForMember(dest => dest.IdSitra, opts => opts.MapFrom(src => src.IdSitra))
                ;
        }
    }

}
