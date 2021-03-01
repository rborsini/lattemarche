using AutoMapper;
using AutoMapper.Configuration;
using LatteMarche.Application.Mobile.Dtos;
using LatteMarche.Common;
using LatteMarche.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Mobile
{
    public class MobileProfile : Profile
    {
        public MobileProfile()
        {

            CreateMap<DispositivoMobile, DispositivoDto>();
            CreateMap<DispositivoDto, DispositivoMobile>();

            CreateMap<Utente, TrasportatoreDto>()
                .ForMember(dest => dest.P_IVA, opt => opt.MapFrom(src => src.PivaCF.Trim()))
                .ForMember(dest => dest.Comune, opt => opt.MapFrom(src => src.Comune.Descrizione.Trim()))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Comune.Provincia.Trim()))
                .ForMember(dest => dest.Indirizzo, opt => opt.MapFrom(src => $"{src.Comune.CAP.Trim()} {src.Indirizzo}"))
            ;

            CreateMap<Autocisterna, AutocisternaDto>();
            CreateMap<Giro, TemplateGiroDto>()
                .ForMember(dest => dest.Allevamenti, opt => opt.Ignore())
                .ForMember(dest => dest.Codice, opt => opt.MapFrom(src => src.CodiceGiro))
                .ForMember(dest => dest.Descrizione, opt => opt.MapFrom(src => src.Denominazione))
                ;

            CreateMap<TipoLatte, TipoLatteDto>();

            CreateMap<Acquirente, AcquirenteDto>()
                .ForMember(dest => dest.P_IVA, opt => opt.MapFrom(src => src.Piva.Trim()))
                .ForMember(dest => dest.Comune, opt => opt.MapFrom(src => src.Comune.Descrizione.Trim()))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Comune.Provincia.Trim()))
                .ForMember(dest => dest.CAP, opt => opt.MapFrom(src => src.Comune.CAP.Trim()))
                ;

            CreateMap<Cessionario, CessionarioDto>()
                .ForMember(dest => dest.Comune, opt => opt.MapFrom(src => src.Comune.Descrizione.Trim()))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Comune.Provincia.Trim()))
                .ForMember(dest => dest.CAP, opt => opt.MapFrom(src => src.Comune.CAP.Trim()))
                .ForMember(dest => dest.P_IVA, opt => opt.MapFrom(src => src.Piva))
                ;

            CreateMap<Destinatario, DestinatarioDto>()
                .ForMember(dest => dest.Comune, opt => opt.MapFrom(src => src.Comune.Descrizione.Trim()))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Comune.Provincia.Trim()))
                .ForMember(dest => dest.CAP, opt => opt.MapFrom(src => src.Comune.CAP.Trim()))
                .ForMember(dest => dest.RagioneSociale, opt => opt.MapFrom(src => $"{src.RagioneSociale} - {src.Stabilimento}" ))
                ;

            CreateMap<PrelievoLatteDto, PrelievoLatte>()
                .ForMember(dest => dest.LastChange, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.LastOperation, opt => opt.MapFrom(src => OperationEnum.Added))
                ;

            CreateMap<TrasbordoDto, Trasbordo>()
                .ForMember(dest => dest.Prelievi_JSON, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Prelievi)))
                ;

            CreateMap<Trasbordo, TrasbordoDto>()
                .ForMember(dest => dest.Prelievi, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<List<PrelievoLatteDto>>(src.Prelievi_JSON)))
                ;

        }
    }
}
