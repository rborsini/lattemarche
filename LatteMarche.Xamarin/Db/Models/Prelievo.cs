using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text;

namespace LatteMarche.Xamarin.Db.Models
{
    public class Prelievo : AbstractEntity<string>
    {

        [Key]
        public override string Id { get; set; }
        public string Titolo { get; set; }
        public DateTime? DataPrelievo { get; set; }
        public string Scomparto { get; set; }
        public decimal? Quantita_kg { get; set; }
        public decimal? Quantita_lt { get; set; }
        public decimal? Temperatura { get; set; }
        public int? NumeroMungiture { get; set; }
        public DateTime? DataConsegna { get; set; }
        public DateTime? DataUltimaMungitura { get; set; }
        
        [ForeignKey(nameof(Giro))]
        public int? IdGiro { get; set; }
        public virtual Giro Giro { get; set; }
        public int? IdAllevamento { get; set; }

        public int? IdAcquirente { get; set; }
        public int? IdCessionario { get; set; }
        public int? IdDestinatario { get; set; }

    }
}
