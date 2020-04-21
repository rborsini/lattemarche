using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Mobile.Dtos
{
    public class DestinatarioDto
    {
        public int Id { get; set; }
        public string RagioneSociale { get; set; }
        public string P_IVA { get; set; }
        public string CAP { get; set; }
        public string Indirizzo { get; set; }
        public string Comune { get; set; }
        public string Provincia { get; set; }
    }
}
