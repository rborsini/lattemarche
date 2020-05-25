using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.AziendeTrasportatori.Dtos
{
    public class AziendaTrasportatoriDto
    {

        public int Id { get; set; }

        public string P_IVA { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string RagioneSociale { get; set; }

    }
}
