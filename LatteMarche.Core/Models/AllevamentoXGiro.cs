using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatteMarche.Core.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("ALLEVAMENTO_X_GIRO")]
    public class AllevamentoXGiro
    {

        [Key, Column("ID_GIRO", Order = 0)]
        public int IdGiro { get; set; }

        [Key, Column("ID_ALLEVAMENTO", Order = 1)]
        public int IdAllevamento { get; set; }

        [Column("PRIORITA")]
        public int? Priorita { get; set; }
    }
}
