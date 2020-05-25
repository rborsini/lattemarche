using AutoMapper.Configuration;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.PrelieviLatte
{

    public class PrelieviLatteMappings
    {
        internal static MapperConfigurationExpression Configure(MapperConfigurationExpression mappings)
        {
            mappings.CreateMap<PrelievoLatte, PrelievoLatteDto>();
            mappings.CreateMap<PrelievoLatteDto, PrelievoLatte>();

            mappings.CreateMap<V_PrelievoLatte, PrelievoLatte>();
            mappings.CreateMap<V_PrelievoLatte, PrelievoLatteDto>();

            mappings.CreateMap<Lotto, LottoDto>();
            mappings.CreateMap<LottoDto, Lotto>();

            mappings.CreateMap<TipoLatte, TipoLatteDto>();
            mappings.CreateMap<TipoLatteDto, TipoLatte>();

            return mappings;
        }
    }

}




