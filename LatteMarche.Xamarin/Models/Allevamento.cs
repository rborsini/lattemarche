using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Models
{
    public class Allevamento : Entity<int>
    {
        public override int Id { get; set; }
        public string CodiceAsl { get; set; }
        public string IndirizzoAllevamento { get; set; }
        public int? IdUtente { get; set; }
        public string SiglaProvincia { get; set; }
        public int IdComune { get; set; }
        public string CUAA { get; set; }
    }
}
