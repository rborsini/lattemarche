using AutoMapper;
using AutoMapper.Configuration;
using LatteMarche.Application.Comuni.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Comuni
{
    public class ComuniProfile : Profile
    {
        public ComuniProfile()
        {
            CreateMap<Comune, ComuneDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Descrizione, opts => opts.MapFrom(src => src.Descrizione.Trim()))
                .ForMember(dest => dest.Provincia, opts => opts.MapFrom(src => src.Provincia.Trim()))
                .ForMember(dest => dest.CAP, opts => opts.MapFrom(src => src.CAP.Trim()))
                ;

        }
    }
}
