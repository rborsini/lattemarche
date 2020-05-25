using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Cessionari.Dtos
{
    public class CessionarioDto
    {
        public int Id { get; set; }
        public string RagioneSociale { get; set; }
        public string Piva { get; set; }
        public string Indirizzo { get; set; }
        public string SiglaProvincia { get; set; }
        public int IdComune { get; set; }
    }
}
