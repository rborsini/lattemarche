using AutoMapper.Configuration;
using LatteMarche.Application.Dispositivi.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dispositivi
{
    public class DispositiviMappings
    {
        private static TimeZoneInfo italyTimeZone => TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");

        internal static MapperConfigurationExpression Configure(MapperConfigurationExpression mappings)
        {
            mappings.CreateMap<DispositivoMobile, DispositivoMobileDto>()
                .ForMember(dest => dest.DataRegistrazione, opts => opts.MapFrom(src => TimeZoneInfo.ConvertTimeFromUtc(src.DataRegistrazione, italyTimeZone)))
                .ForMember(dest => dest.DataUltimoDownload, opts => opts.MapFrom(src => src.DataUltimoDownload.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(src.DataUltimoDownload.Value, italyTimeZone) : (DateTime?)null))
                .ForMember(dest => dest.DataUltimoUpload, opts => opts.MapFrom(src => src.DataUltimoUpload.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(src.DataUltimoUpload.Value, italyTimeZone) : (DateTime?)null))
                ;

            mappings.CreateMap<DispositivoMobileDto, DispositivoMobile>();

            return mappings;
        }
    }
}
