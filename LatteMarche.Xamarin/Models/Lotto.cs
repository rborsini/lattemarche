using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Models
{
    public class Lotto
    {
        public string Codice { get; set; }

        public List<Prelievo> Prelievi { get; set; }

        public Lotto()
        {
            this.Prelievi = new List<Prelievo>();
        }


    }
}
