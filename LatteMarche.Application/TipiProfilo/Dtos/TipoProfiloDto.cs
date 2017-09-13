using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.TipiProfilo.Dtos
{
    public class TipoProfiloDto : EntityDto
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
        
    }

    public class TipiProfiloMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<TipoProfilo, TipoProfiloDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Descrizione, opts => opts.MapFrom(src => src.Descrizione.Trim()))
                ;
        }
    }

}
