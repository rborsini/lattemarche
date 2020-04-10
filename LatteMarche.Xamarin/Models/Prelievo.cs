using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LatteMarche.Xamarin.Models
{
    public class Prelievo : Entity<string>
    {
        [Key]
        public override string Id { get; set; }
        public DateTime? DataPrelievo { get; set; }
        public string Scomparto { get; set; }
        public decimal? Quantita_kg { get; set; }
        public decimal? Quantita_lt { get; set; }
        public decimal? Temperatura { get; set; }
        public int? NumeroMungiture { get; set; }
        public DateTime? DataConsegna { get; set; }
        public DateTime? DataUltimaMungitura { get; set; }

        [ForeignKey("Allevamento")]
        public int? IdAllevamento { get; set; } 

        public virtual Allevamento Allevamento { get; set; }

        [ForeignKey("TipoLatte")]
        public int? IdTipoLatte { get; set; }

        public virtual TipoLatte TipoLatte { get; set; }

    }
}
