using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Trasportatori.Dtos
{
    public class GiroItemDto
    {
        public int IdGiro { get; set; }

        public int IdAllevamento { get; set; }

        public string Allevatore { get; set; }

        public string RagioneSociale { get; set; }

        public string Indirizzo { get; set; }

        public bool? Selezionato { get; set; }

        public int? Priorita { get; set; }

    }
}
