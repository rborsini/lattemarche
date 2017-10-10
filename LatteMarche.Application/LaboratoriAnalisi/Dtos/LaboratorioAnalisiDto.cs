using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.LaboratoriAnalisi.Dtos
{
    public class LaboratorioAnalisiDto : EntityDto
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
        
    }

    public class LaboratoriAnalisiMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<LaboratorioAnalisi, LaboratorioAnalisiDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Descrizione, opts => opts.MapFrom(src => src.Descrizione.Trim()))
                ;
        }
    }

}
