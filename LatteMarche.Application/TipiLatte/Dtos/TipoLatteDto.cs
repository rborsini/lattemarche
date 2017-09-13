using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.TipiLatte.Dtos
{
    public class TipoLatteDto : EntityDto
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
        public string DescrizioneBreve { get; set; }
        
    }

    public class TipiLatteMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<TipoLatte, TipoLatteDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Descrizione, opts => opts.MapFrom(src => src.Descrizione.Trim()))
                .ForMember(dest => dest.DescrizioneBreve, opts => opts.MapFrom(src => src.DescrizioneBreve.Trim()))
                ;
        }
    }

}
