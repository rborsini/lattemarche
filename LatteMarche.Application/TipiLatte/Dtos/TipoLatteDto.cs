using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.TipiLatte.Dtos
{
    public class TipoLatteDto : EntityDto
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
        public string DescrizioneBreve { get; set; }
        public decimal FattoreConversione { get; set; }
        public bool FlagInvioSitra { get; set; }
    }

    public class TipiLatteMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<TipoLatte, TipoLatteDto>();
            Mapper.CreateMap<TipoLatteDto, TipoLatte>();
        }
    }

}
