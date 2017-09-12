using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Comuni.Dtos
{
    public class ComuneDto : EntityDto
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
        public string Provincia { get; set; }
        public string CAP { get; set; }
        
    }

    public class ComuniMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<Comune, ComuneDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Descrizione, opts => opts.MapFrom(src => src.Descrizione.Trim()))
                .ForMember(dest => dest.Provincia, opts => opts.MapFrom(src => src.Provincia.Trim()))
                .ForMember(dest => dest.CAP, opts => opts.MapFrom(src => src.CAP.Trim()))
                ;
        }
    }

}
