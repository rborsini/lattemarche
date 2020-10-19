using AutoMapper;
using AutoMapper.Configuration;
using LatteMarche.Application.Giri.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Giri
{
    public class GiriProfile : Profile
    {
        public GiriProfile()
        {

            CreateMap<Giro, GiroDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Denominazione, opts => opts.MapFrom(src => src.Denominazione.Trim()))
                .ForMember(dest => dest.CodiceGiro, opts => opts.MapFrom(src => src.CodiceGiro.Trim()))
                .ForMember(dest => dest.IdTrasportatore, opts => opts.MapFrom(src => src.IdTrasportatore))
                ;

            CreateMap<GiroDto, Giro>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Denominazione, opts => opts.MapFrom(src => src.Denominazione.Trim()))
                .ForMember(dest => dest.CodiceGiro, opts => opts.MapFrom(src => src.CodiceGiro.Trim()))
                .ForMember(dest => dest.IdTrasportatore, opts => opts.MapFrom(src => src.IdTrasportatore))
                ;

        }
    }
}
