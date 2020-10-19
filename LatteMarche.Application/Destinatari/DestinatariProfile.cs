using AutoMapper;
using AutoMapper.Configuration;
using LatteMarche.Application.Destinatari.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Destinatari
{
    public class DestinatariProfile : Profile
    {
        public DestinatariProfile()
        {
            CreateMap<Destinatario, DestinatarioDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.RagioneSociale, opts => opts.MapFrom(src => src.RagioneSociale.Trim()))
                .ForMember(dest => dest.P_IVA, opts => opts.MapFrom(src => src.P_IVA.Trim()))
                .ForMember(dest => dest.Indirizzo, opts => opts.MapFrom(src => src.Indirizzo.Trim()))
                .ForMember(dest => dest.IdComune, opts => opts.MapFrom(src => src.IdComune))
                .ForMember(dest => dest.SiglaProvincia, opts => opts.MapFrom(src => src.Comune.Provincia))
                ;

            CreateMap<DestinatarioDto, Destinatario>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.RagioneSociale, opts => opts.MapFrom(src => src.RagioneSociale.Trim()))
                .ForMember(dest => dest.P_IVA, opts => opts.MapFrom(src => src.P_IVA.Trim()))
                .ForMember(dest => dest.Indirizzo, opts => opts.MapFrom(src => src.Indirizzo.Trim()))
                .ForMember(dest => dest.IdComune, opts => opts.MapFrom(src => src.IdComune))
                .ForMember(dest => dest.Abilitato, opts => opts.MapFrom(src => true))
                ;

        }
    }
}
