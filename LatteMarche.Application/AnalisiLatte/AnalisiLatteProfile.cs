using AutoMapper;
using AutoMapper.Configuration;
using LatteMarche.Application.AnalisiLatte.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.AnalisiLatte
{
    public class AnalisiLatteProfile : Profile
    {
        public AnalisiLatteProfile()
        {
            CreateMap<Assam.Models.Misura, ValoreAnalisiDto>();

            CreateMap<Assam.Models.AnalisiLatte, AnalisiDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Campione.Trim()))
                ;

            CreateMap<Analisi, AnalisiDto>();
            CreateMap<ValoreAnalisi, ValoreAnalisiDto>();

            CreateMap<AnalisiDto, Analisi>();
            CreateMap<ValoreAnalisiDto, ValoreAnalisi>();

            CreateMap<LaboratorioAnalisi, LaboratorioAnalisiDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Descrizione, opts => opts.MapFrom(src => src.Descrizione.Trim()))
                ;

        }
    }
}
