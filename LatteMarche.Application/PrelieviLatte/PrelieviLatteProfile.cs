using AutoMapper;
using AutoMapper.Configuration;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.PrelieviLatte
{

    public class PrelieviLatteProfile : Profile
    {
        public PrelieviLatteProfile()
        {
            CreateMap<PrelievoLatte, PrelievoLatteDto>();
            CreateMap<PrelievoLatteDto, PrelievoLatte>();

            CreateMap<V_PrelievoLatte, PrelievoLatte>();
            CreateMap<V_PrelievoLatte, PrelievoLatteDto>();

            CreateMap<Lotto, LottoDto>();
            CreateMap<LottoDto, Lotto>();

            CreateMap<TipoLatte, TipoLatteDto>();
            CreateMap<TipoLatteDto, TipoLatte>();

        }
    }

}




