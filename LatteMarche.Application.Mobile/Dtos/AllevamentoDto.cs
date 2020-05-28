using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Mobile.Dtos
{
    public class AllevamentoDto
    {
        public int IdAllevamento { get; set; }
        public string RagioneSociale { get; set; }
        public string P_IVA { get; set; }
        public string CAP { get; set; }
        public string Indirizzo { get; set; }
        public string Comune { get; set; }
        public string Provincia { get; set; }

        public double? Latitudine { get; set; }
        public double? Longitudine { get; set; }

        public int? IdAcquirenteDefault { get; set; }
        public int? IdDestinatarioDefault { get; set; }
        public int? IdCessionarioDefault { get; set; }

        public decimal? Quantita_Min { get; set; }
        public decimal? Quantita_Max { get; set; }

        public decimal? Temperatura_Min { get; set; }
        public decimal? Temperatura_Max { get; set; }

        public int? IdTipoLatte { get; set; }

        public int? Priorita { get; set; }

        public int? IdTemplateGiro { get; set; }
    }
}
