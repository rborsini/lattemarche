using AutoMapper.Configuration;
using LatteMarche.Application.Auth.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Auth
{
    public class AuthMappings
    {
        internal static MapperConfigurationExpression Configure(MapperConfigurationExpression mappings)
        {
            mappings.CreateMap<Azione, AzioneDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => src.Type.Trim()))
                .ForMember(dest => dest.Controller, opts => opts.MapFrom(src => src.Controller.Trim()))
                .ForMember(dest => dest.Action, opts => opts.MapFrom(src => src.Action.Trim()))
                .ForMember(dest => dest.ViewItem, opts => opts.MapFrom(src => src.ViewItem.Trim()))
                .ForMember(dest => dest.Pagina, opts => opts.MapFrom(src => src.Pagina.Trim()))
                .ForMember(dest => dest.Nome, opts => opts.MapFrom(src => src.Nome.Trim()))
                ;

            mappings.CreateMap<AzioneDto, Azione>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => src.Type.Trim()))
                .ForMember(dest => dest.Controller, opts => opts.MapFrom(src => src.Controller.Trim()))
                .ForMember(dest => dest.Action, opts => opts.MapFrom(src => src.Action.Trim()))
                .ForMember(dest => dest.ViewItem, opts => opts.MapFrom(src => src.ViewItem.Trim()))
                .ForMember(dest => dest.Pagina, opts => opts.MapFrom(src => src.Pagina.Trim()))
                .ForMember(dest => dest.Nome, opts => opts.MapFrom(src => src.Nome.Trim()))
                ;

            mappings.CreateMap<Ruolo, RuoloDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Codice, opts => opts.MapFrom(src => src.Codice.Trim()))
                .ForMember(dest => dest.Descrizione, opts => opts.MapFrom(src => src.Descrizione.Trim()))
                ;

            mappings.CreateMap<RuoloDto, Ruolo>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Codice, opts => opts.MapFrom(src => src.Codice.Trim()))
                .ForMember(dest => dest.Descrizione, opts => opts.MapFrom(src => src.Descrizione.Trim()))
                ;

            return mappings;
        }
    }
}
