using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LatteMarche.Synch.DataType
{
    public class Prelievo
    {

        public int Id { get; set; }
        public int? IdAllevamento { get; set; }
        public int? IdDestinatario { get; set; }
        public int? IdAcquirente { get; set; }
        public int? IdTrasportatore { get; set; }
        public int? IdLabAnalisi { get; set; }
        public DateTime? DataPrelievo { get; set; }
        public DateTime? DataConsegna { get; set; }
        public DateTime? DataUltimaMungitura { get; set; }
        public Decimal? Quantita { get; set; }
        public Decimal? Temperatura { get; set; }
        public int? NumeroMungiture { get; set; }
        public string Scomparto { get; set; }
        public string LottoConsegna { get; set; }
        public string SerialeLabAnalisi { get; set; }

        public OperationEnum LastOperation { get; set; }
    }
}
