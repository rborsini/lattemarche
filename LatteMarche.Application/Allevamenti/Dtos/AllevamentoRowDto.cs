using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Allevamenti.Dtos
{
    public class AllevamentoRowDto
    {
        public int Id { get; set; }

        public string CodiceAsl { get; set; }

        public string Utente_CodiceAllevatore { get; set; }

        public string IndirizzoAllevamento { get; set; }

        public int IdUtente { get; set; }

        public int IdComune { get; set; }

        public string CUAA { get; set; }

        public string RagioneSociale { get; set; }

        public int? Utente_IdTipoLatte { get; set; }
    }
}
