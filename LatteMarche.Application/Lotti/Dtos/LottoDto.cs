using AutoMapper;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LatteMarche.Application.PrelieviLatte.Dtos;

namespace LatteMarche.Application.Lotti.Dtos
{
    public class LottoDto : EntityDto
    {
        public long Id { get; set; }

        public string Codice { get; set; }

        public string CodiceSitra { get; set; }

        public DateTime TimeStamp { get; set; }

        public bool Inviato { get; set; }

        public bool Errore { get; set; }

        public string Messaggio { get; set; }

        public decimal Quantita { get; set; }

        public DateTime DataUltimaMungitura { get; set; }

        public DateTime DataConsegna { get; set; }

        public List<PrelievoLatteDto> PrelieviPadre { get; set; }

        public LottoDto()
        {
            this.PrelieviPadre = new List<PrelievoLatteDto>();
        }

    }

    public class LottiMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<Lotto, LottoDto>();
            Mapper.CreateMap<LottoDto, Lotto>();
        }
    }

}
