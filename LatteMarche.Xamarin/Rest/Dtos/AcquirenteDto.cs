using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Rest.Dtos
{
    public class AcquirenteDto 
    {
        public int Id { get; set; }
        public string RagioneSociale { get; set; }
        public string P_IVA { get; set; }
        public string Indirizzo { get; set; }
        public string Comune { get; set; }
        public string Provincia { get; set; }
    }
}
