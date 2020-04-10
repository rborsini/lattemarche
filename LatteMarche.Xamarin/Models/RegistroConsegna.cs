using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Models
{
    public class RegistroConsegna : Registro
    {
        public Prelievo Prelievo { get; set; }

        public string Comunicazione { get; set; }

        public RegistroConsegna()
        {
            this.Titolo = "Registro consegna latte bovino";
        }
    }
}
