using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
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
        
        [ForeignKey("Lotto")]
        public string IdLotto { get; set; }
        public virtual Lotto Lotto { get; set; }

        public int? IdAllevamento { get; set; }

        [ForeignKey("TipoLatte")]
        public int? IdTipoLatte { get; set; }
        public virtual TipoLatte TipoLatte { get; set; }

        public int? IdAcquirente { get; set; }
        public int? IdDestinatario { get; set; }

    }
}
