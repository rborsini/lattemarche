using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Models
{
    public class RegistroRaccolta : Registro
    {
        public Lotto Lotto { get; set; }

        public Prelievo Prelievo { get; set; }

        public string Comunicazioni { get; set; }

        public RegistroRaccolta()
        {
            this.Titolo = "Registro raccolta latte bovino";
            this.Comunicazioni = "Latte crudo destinato alla produzione di latte fresco pastorizzato di Alta Qualità in possesso dei requisiti di composizione igienico-sanitari previsti dal D.M. 185/91";
        }
    }
}
