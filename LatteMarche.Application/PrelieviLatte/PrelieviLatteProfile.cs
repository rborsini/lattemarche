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
            
            CreateMap<V_PrelievoLatte, PrelievoLatteDto>()
                .ForMember(dest => dest.DataPrelievoStr, opt => opt.Ignore())
                .ForMember(dest => dest.DataUltimaMungituraStr, opt => opt.Ignore())
                .ForMember(dest => dest.DataConsegnaStr, opt => opt.Ignore())
                ;

            CreateMap<Lotto, LottoDto>();
            CreateMap<LottoDto, Lotto>();

            CreateMap<TipoLatte, TipoLatteDto>();
            CreateMap<TipoLatteDto, TipoLatte>();

        }
    }

}




