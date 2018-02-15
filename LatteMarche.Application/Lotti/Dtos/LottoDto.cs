using AutoMapper;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
