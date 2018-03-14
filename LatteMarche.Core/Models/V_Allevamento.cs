using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatteMarche.Core.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("V_Allevamenti")]
    public class V_Allevamento : Entity<int>
    {
        [Key]
        [Column("ID_ALLEVAMENTO")]
        public override int Id { get; set; }

        [Column("ID_TIPO_LATTE")]
        public int IdTipoLatte { get; set; }

        [Column("FATTORE_CONVERSIONE")]
        public decimal FattoreConversione { get; set; }

        [Column("FLAG_INVIO_SITRA")]
        public bool FlagInvioSitra { get; set; }
    }
}
