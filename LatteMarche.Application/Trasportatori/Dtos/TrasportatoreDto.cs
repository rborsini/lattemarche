using System;
using AutoMapper;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using LatteMarche.Application.Giri.Dtos;

namespace LatteMarche.Application.Trasportatori.Dtos
{
    public class TrasportatoreDto : EntityDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Indirizzo { get; set; }
        public string Telefono { get; set; }
        public string Cellulare { get; set; }
        public string Comune { get; set; }       
        public string Provincia { get; set; }

        public List<GiroDto> Giri { get; set; }

    }

    public class TrasportatoriMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<V_Trasportatore, TrasportatoreDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opts => opts.MapFrom(src => src.Nome.Trim()))
                .ForMember(dest => dest.Cognome, opts => opts.MapFrom(src => src.Cognome.Trim()))
                .ForMember(dest => dest.Indirizzo, opts => opts.MapFrom(src => src.Indirizzo.Trim()))
                .ForMember(dest => dest.Telefono, opts => opts.MapFrom(src => src.Telefono.Trim()))
                .ForMember(dest => dest.Cellulare, opts => opts.MapFrom(src => src.Cellulare.Trim()))
                .ForMember(dest => dest.Comune, opts => opts.MapFrom(src => src.Comune.Trim()))
                .ForMember(dest => dest.Provincia, opts => opts.MapFrom(src => src.Provincia.Trim()))
                ;
            Mapper.CreateMap<TrasportatoreDto, V_Trasportatore>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opts => opts.MapFrom(src => src.Nome.Trim()))
                .ForMember(dest => dest.Cognome, opts => opts.MapFrom(src => src.Cognome.Trim()))
                .ForMember(dest => dest.Indirizzo, opts => opts.MapFrom(src => src.Indirizzo.Trim()))
                .ForMember(dest => dest.Telefono, opts => opts.MapFrom(src => src.Telefono.Trim()))
                .ForMember(dest => dest.Cellulare, opts => opts.MapFrom(src => src.Cellulare.Trim()))
                .ForMember(dest => dest.Comune, opts => opts.MapFrom(src => src.Comune.Trim()))
                .ForMember(dest => dest.Provincia, opts => opts.MapFrom(src => src.Provincia.Trim()))
                ;
        }
    }

}
