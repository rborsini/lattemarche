using AutoMapper.Configuration;
using LatteMarche.Application.Trasportatori.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Trasportatori
{


    public class TrasportatoriMappings
    {
        public static MapperConfigurationExpression Configure(MapperConfigurationExpression mappings)
        {
            mappings.CreateMap<Autocisterna, AutocisternaDto>();
            mappings.CreateMap<AutocisternaDto, Autocisterna>();

            mappings.CreateMap<V_Trasportatore, TrasportatoreDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opts => opts.MapFrom(src => src.Nome.Trim()))
                .ForMember(dest => dest.Cognome, opts => opts.MapFrom(src => src.Cognome.Trim()))
                .ForMember(dest => dest.Indirizzo, opts => opts.MapFrom(src => src.Indirizzo.Trim()))
                .ForMember(dest => dest.Telefono, opts => opts.MapFrom(src => src.Telefono.Trim()))
                .ForMember(dest => dest.Cellulare, opts => opts.MapFrom(src => src.Cellulare.Trim()))
                .ForMember(dest => dest.Comune, opts => opts.MapFrom(src => src.Comune.Trim()))
                .ForMember(dest => dest.Provincia, opts => opts.MapFrom(src => src.Provincia.Trim()))
                .ForMember(dest => dest.P_IVA, opts => opts.MapFrom(src => src.P_IVA.Trim()))
                .ForMember(dest => dest.RagioneSociale, opts => opts.MapFrom(src => src.RagioneSociale.Trim()))
                ;
            mappings.CreateMap<TrasportatoreDto, V_Trasportatore>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opts => opts.MapFrom(src => src.Nome.Trim()))
                .ForMember(dest => dest.Cognome, opts => opts.MapFrom(src => src.Cognome.Trim()))
                .ForMember(dest => dest.Indirizzo, opts => opts.MapFrom(src => src.Indirizzo.Trim()))
                .ForMember(dest => dest.Telefono, opts => opts.MapFrom(src => src.Telefono.Trim()))
                .ForMember(dest => dest.Cellulare, opts => opts.MapFrom(src => src.Cellulare.Trim()))
                .ForMember(dest => dest.Comune, opts => opts.MapFrom(src => src.Comune.Trim()))
                .ForMember(dest => dest.Provincia, opts => opts.MapFrom(src => src.Provincia.Trim()))
                .ForMember(dest => dest.P_IVA, opts => opts.MapFrom(src => src.P_IVA.Trim()))
                .ForMember(dest => dest.RagioneSociale, opts => opts.MapFrom(src => src.RagioneSociale.Trim()))
                ;

            mappings.CreateMap<Giro, GiroDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Denominazione, opts => opts.MapFrom(src => src.Denominazione.Trim()))
                .ForMember(dest => dest.CodiceGiro, opts => opts.MapFrom(src => src.CodiceGiro.Trim()))
                .ForMember(dest => dest.IdTrasportatore, opts => opts.MapFrom(src => src.IdTrasportatore))
                ;

            mappings.CreateMap<GiroDto, Giro>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Denominazione, opts => opts.MapFrom(src => src.Denominazione.Trim()))
                .ForMember(dest => dest.CodiceGiro, opts => opts.MapFrom(src => src.CodiceGiro.Trim()))
                .ForMember(dest => dest.IdTrasportatore, opts => opts.MapFrom(src => src.IdTrasportatore))
                ;

            return mappings;
        }
    }

}
