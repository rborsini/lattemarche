using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatteMarche.Core.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("ALLEVAMENTO_X_GIRO")]
    public class AllevamentoXGiro 
    {

        [ForeignKey(nameof(Giro))]
        [Key, Column("ID_GIRO", Order = 0)]
        public int IdGiro { get; set; }

        public virtual Giro Giro { get; set; }

        [ForeignKey(nameof(Allevamento))]
        [Key, Column("ID_ALLEVAMENTO", Order = 1)]
        public int IdAllevamento { get; set; }

        public virtual Allevamento Allevamento { get; set; }

        [Column("PRIORITA")]
        public int? Priorita { get; set; }
    }
}
