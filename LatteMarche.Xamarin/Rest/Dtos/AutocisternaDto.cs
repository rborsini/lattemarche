using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Rest.Dtos
{
    public class AutocisternaDto 
    {
        public int Id { get; set; }

        public string Marca { get; set; }
        public string Modello { get; set; }
        public string Targa { get; set; }

        public int? IdTrasportatore { get; set; }
        public int? Portata { get; set; }
        public int? NumScomparti { get; set; }

    }
}
