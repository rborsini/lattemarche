using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Rest.Dtos
{
    public class TrasportatoreDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Indirizzo { get; set; }
        public string Telefono { get; set; }
        public string Cellulare { get; set; }
        public string Comune { get; set; }
        public string Provincia { get; set; }
        public string P_IVA { get; set; }
        public string RagioneSociale { get; set; }

    }
}
