using System;
using System.Collections.Generic;

namespace LatteMarche.Application.Latte.Dtos
{
    public class LottoDto 
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

}
