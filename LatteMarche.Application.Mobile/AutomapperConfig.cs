using AutoMapper.Configuration;
using LatteMarche.Application.Mobile.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Mobile
{
    public static class AutomapperConfig
    {
        public static MapperConfigurationExpression Configure(MapperConfigurationExpression mappings)
        {

            mappings.CreateMap<DispositivoMobile, DispositivoDto>();
            mappings.CreateMap<DispositivoDto, DispositivoMobile>();

            mappings.CreateMap<V_Trasportatore, TrasportatoreDto>()
                .ForMember(dest => dest.Comune, opt => opt.MapFrom(src => src.Comune.Trim()));
            ;
            mappings.CreateMap<Autocisterna, AutocisternaDto>();
            mappings.CreateMap<Giro, TemplateGiroDto>()
                .ForMember(dest => dest.Allevamenti, opt => opt.Ignore());

            mappings.CreateMap<TipoLatte, TipoLatteDto>();

            mappings.CreateMap<Acquirente, AcquirenteDto>()
                .ForMember(dest => dest.P_IVA, opt => opt.MapFrom(src => src.Piva.Trim()))
                .ForMember(dest => dest.Comune, opt => opt.MapFrom(src => src.Comune.Descrizione.Trim()))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Comune.Provincia.Trim()))
                ;

            mappings.CreateMap<Destinatario, DestinatarioDto>()
                .ForMember(dest => dest.Comune, opt => opt.MapFrom(src => src.Comune.Descrizione.Trim()))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Comune.Provincia.Trim()))
                ;

            return mappings;

        }
    }
}
