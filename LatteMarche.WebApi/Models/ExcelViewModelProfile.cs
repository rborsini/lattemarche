using AutoMapper;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LatteMarche.WebApi.Models
{
    public class ExcelViewModelProfile : Profile
    {
        public ExcelViewModelProfile()
        {
            CreateMap<V_PrelievoLatte, ExcelTrasportatoriViewModel>()
                .ForMember(dest => dest.Quantita_Kg, opts => opts.MapFrom(src => src.Quantita))
                .ForMember(dest => dest.Quantita_Lt, opts => opts.MapFrom(src => src.QuantitaLitri))
                ;
        }
    }
}