using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Models
{
    public abstract class Registro
    {
        public DateTime Data { get; set; }

        public string Header_1 => "latte marche";
        public string Header_2 => "Organizzazione Produttori";
        public string Titolo { get; set; }
        public string SottoTitolo => "L.119/03-D.M. 31/07/03, art.12 - Documentazione raccolta latte - Sistema informatizzato di registrazione - Autorizzazione Regione Marche DDs 512/SAR";

        public Acquirente Acquirente { get; set; }

        public Destinatario Destinatario { get; set; }

        public Trasportatore Trasportatore { get; set; }

        public Giro Giro { get; set; }

    }
}
