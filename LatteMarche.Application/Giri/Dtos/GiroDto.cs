using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Giri.Dtos
{
    public class GiroDto : EntityDto
    {
        public int Id { get; set; }
        public string Denominazione { get; set; }
        public string CodiceGiro { get; set; }
        public int IdTrasportatore { get; set; }
    }

    public class GiriMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<Giro, GiroDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Denominazione, opts => opts.MapFrom(src => src.Denominazione.Trim()))
                .ForMember(dest => dest.CodiceGiro, opts => opts.MapFrom(src => src.CodiceGiro.Trim()))
                .ForMember(dest => dest.IdTrasportatore, opts => opts.MapFrom(src => src.IdTrasportatore))
                ;

            Mapper.CreateMap<GiroDto, Giro>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Denominazione, opts => opts.MapFrom(src => src.Denominazione.Trim()))
                .ForMember(dest => dest.CodiceGiro, opts => opts.MapFrom(src => src.CodiceGiro.Trim()))
                .ForMember(dest => dest.IdTrasportatore, opts => opts.MapFrom(src => src.IdTrasportatore))
                ;
        }
    }

}
