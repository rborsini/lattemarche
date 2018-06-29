using AutoMapper;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Ruoli.Dtos
{
    public class AzioneDto : EntityDto
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ViewItem { get; set; }
        public string Pagina { get; set; }
        public string Nome { get; set; }
    }

    public class AzioneMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<Azione, AzioneDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => src.Type.Trim()))
                .ForMember(dest => dest.Controller, opts => opts.MapFrom(src => src.Controller.Trim()))
                .ForMember(dest => dest.Action, opts => opts.MapFrom(src => src.Action.Trim()))
                .ForMember(dest => dest.ViewItem, opts => opts.MapFrom(src => src.ViewItem.Trim()))
                .ForMember(dest => dest.Pagina, opts => opts.MapFrom(src => src.Pagina.Trim()))
                .ForMember(dest => dest.Nome, opts => opts.MapFrom(src => src.Nome.Trim()))
                ;

            Mapper.CreateMap<AzioneDto, Azione>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => src.Type.Trim()))
                .ForMember(dest => dest.Controller, opts => opts.MapFrom(src => src.Controller.Trim()))
                .ForMember(dest => dest.Action, opts => opts.MapFrom(src => src.Action.Trim()))
                .ForMember(dest => dest.ViewItem, opts => opts.MapFrom(src => src.ViewItem.Trim()))
                .ForMember(dest => dest.Pagina, opts => opts.MapFrom(src => src.Pagina.Trim()))
                .ForMember(dest => dest.Nome, opts => opts.MapFrom(src => src.Nome.Trim()))
                ;
        }
    }
}
