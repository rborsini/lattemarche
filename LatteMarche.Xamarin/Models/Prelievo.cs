using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Models
{
    public class Prelievo : Entity<string>
    {
        public override string Id { get; set; }
        public DateTime? DataPrelievo { get; set; }
        public string Scomparto { get; set; }
        public decimal? Quantita { get; set; }
        public decimal? Temperatura { get; set; }
        public int? NumeroMungiture { get; set; }
        public DateTime? DataConsegna { get; set; }
        public DateTime? DataUltimaMungitura { get; set; }
    }
}
