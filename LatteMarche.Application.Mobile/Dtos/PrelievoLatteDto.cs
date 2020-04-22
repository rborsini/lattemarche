using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Mobile.Dtos
{
    public class PrelievoLatteDto
    {
        public DateTime DataPrelievo { get; set; }

        public DateTime DataConsegna { get; set; }

        public decimal? Quantita { get; set; }

        public decimal? Temperatura { get; set; }

        public DateTime DataUltimaMungitura { get; set; }

        public int IdAllevamento { get; set; }

        public int IdDestinatario { get; set; }

        public int IdAcquirente { get; set; }

        public int IdLabAnalisi { get; set; }

        public int IdTrasportatore { get; set; }

        public int NumeroMungiture { get; set; }

        public string Scomparto { get; set; }

        public string LottoConsegna { get; set; }

    }
}
