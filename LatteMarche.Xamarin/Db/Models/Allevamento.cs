using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LatteMarche.Xamarin.Db.Models
{
    public class Allevamento : AbstractEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        public int IdAllevamento { get; set; }

        public string RagioneSociale { get; set; }
        public string P_IVA { get; set; }
        public string CAP { get; set; }
        public string Indirizzo { get; set; }
        public string Comune { get; set; }
        public string Provincia { get; set; }

        [ForeignKey(nameof(TemplateGiro))]
        public int? IdTemplateGiro { get; set; }
        public TemplateGiro TemplateGiro { get; set; }
        public int Priorita { get; set; }

        [ForeignKey(nameof(TipoLatte))]
        public int? IdTipoLatte { get; set; }
        public TipoLatte TipoLatte { get; set; }

        public double? Latitudine { get; set; }
        public double? Longitudine { get; set; }

        public int? IdAcquirenteDefault { get; set; }
        public int? IdCessionarioDefault { get; set; }
        public int? IdDestinatarioDefault { get; set; }

        public decimal? Quantita_Min { get; set; }
        public decimal? Quantita_Max { get; set; }

        public decimal? Temperatura_Min { get; set; }
        public decimal? Temperatura_Max { get; set; }



    }
}
