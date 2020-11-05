using AutoMapper;
using AutoMapper.Configuration;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Allevamenti
{
    public class AllevamentiProfile : Profile
    {
        public AllevamentiProfile()
        {
            // https://docs.automapper.org/en/stable/Reverse-Mapping-and-Unflattening.html
            CreateMap<Allevamento, AllevamentoDto>()
                .ForMember(dest => dest.SiglaProvincia, opts => opts.MapFrom(src => src.Comune.Provincia));

            CreateMap<AllevamentoDto, Allevamento>()
                .ForMember(dest => dest.Abilitato, opts => opts.MapFrom(src => true));
            
        }
    }
}
