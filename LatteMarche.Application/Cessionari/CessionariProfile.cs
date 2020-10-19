using AutoMapper;
using AutoMapper.Configuration;
using LatteMarche.Application.Cessionari.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Cessionari
{
    public class CessionariProfile : Profile
    {
        public CessionariProfile()
        {
            CreateMap<Cessionario, CessionarioDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.RagioneSociale, opts => opts.MapFrom(src => src.RagioneSociale.Trim()))
                .ForMember(dest => dest.Piva, opts => opts.MapFrom(src => src.Piva.Trim()))
                .ForMember(dest => dest.Indirizzo, opts => opts.MapFrom(src => src.Indirizzo.Trim()))
                .ForMember(dest => dest.IdComune, opts => opts.MapFrom(src => src.IdComune))
                .ForMember(dest => dest.SiglaProvincia, opts => opts.MapFrom(src => src.Comune.Provincia))
                ;
            
            CreateMap<CessionarioDto, Cessionario>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.RagioneSociale, opts => opts.MapFrom(src => src.RagioneSociale.Trim()))
                .ForMember(dest => dest.Piva, opts => opts.MapFrom(src => src.Piva.Trim()))
                .ForMember(dest => dest.Indirizzo, opts => opts.MapFrom(src => src.Indirizzo.Trim()))
                .ForMember(dest => dest.IdComune, opts => opts.MapFrom(src => src.IdComune))
                .ForMember(dest => dest.Abilitato, opts => opts.MapFrom(src => true))
                ;

        }
    }
}
