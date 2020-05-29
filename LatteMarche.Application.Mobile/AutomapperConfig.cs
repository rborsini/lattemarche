using AutoMapper.Configuration;
using LatteMarche.Application.Mobile.Dtos;
using LatteMarche.Common;
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

            mappings.CreateMap<Utente, TrasportatoreDto>()
                .ForMember(dest => dest.P_IVA, opt => opt.MapFrom(src => src.PivaCF.Trim()))
                .ForMember(dest => dest.Comune, opt => opt.MapFrom(src => src.Comune.Descrizione.Trim()))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Comune.Provincia.Trim()))
                .ForMember(dest => dest.Indirizzo, opt => opt.MapFrom(src => $"{src.Comune.CAP.Trim()} {src.Indirizzo}"))
            ;

            mappings.CreateMap<Autocisterna, AutocisternaDto>();
            mappings.CreateMap<Giro, TemplateGiroDto>()
                .ForMember(dest => dest.Allevamenti, opt => opt.Ignore())
                .ForMember(dest => dest.Codice, opt => opt.MapFrom(src => src.CodiceGiro))
                .ForMember(dest => dest.Descrizione, opt => opt.MapFrom(src => src.Denominazione))
                ;

            mappings.CreateMap<TipoLatte, TipoLatteDto>();

            mappings.CreateMap<Acquirente, AcquirenteDto>()
                .ForMember(dest => dest.P_IVA, opt => opt.MapFrom(src => src.Piva.Trim()))
                .ForMember(dest => dest.Comune, opt => opt.MapFrom(src => src.Comune.Descrizione.Trim()))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Comune.Provincia.Trim()))
                .ForMember(dest => dest.CAP, opt => opt.MapFrom(src => src.Comune.CAP.Trim()))
                ;

            mappings.CreateMap<Cessionario, CessionarioDto>()
                .ForMember(dest => dest.Comune, opt => opt.MapFrom(src => src.Comune.Descrizione.Trim()))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Comune.Provincia.Trim()))
                .ForMember(dest => dest.CAP, opt => opt.MapFrom(src => src.Comune.CAP.Trim()))
                .ForMember(dest => dest.P_IVA, opt => opt.MapFrom(src => src.Piva))
                ;

            mappings.CreateMap<Destinatario, DestinatarioDto>()
                .ForMember(dest => dest.Comune, opt => opt.MapFrom(src => src.Comune.Descrizione.Trim()))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Comune.Provincia.Trim()))
                .ForMember(dest => dest.CAP, opt => opt.MapFrom(src => src.Comune.CAP.Trim()))
                ;

            mappings.CreateMap<PrelievoLatteDto, PrelievoLatte>()
                .ForMember(dest => dest.LastChange, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.LastOperation, opt => opt.MapFrom(src => OperationEnum.Added))
                ;

            return mappings;

        }
    }
}
