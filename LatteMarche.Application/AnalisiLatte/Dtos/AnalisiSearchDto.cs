using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.AnalisiLatte.Dtos
{
    public class AnalisiSearchDto
    {
        public int? IdProduttore { get; set; }

        public string CodiceProduttore { get; set; }

        public int? IdAllevamento { get; set; }

        public string CodiceAsl { get; set; }

        public string Campione { get; set; }

    }
}
