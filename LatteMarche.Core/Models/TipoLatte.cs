using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeCode.Data;

namespace LatteMarche.Core.Models
{
	[System.ComponentModel.DataAnnotations.Schema.Table("TIPO_LATTE")]
    public class TipoLatte : Entity<int>
    {
        [Key]
        [Column("ID_TIPO_LATTE")]
        public override int Id { get; set; }

        [Column("DESCRIZIONE")]
        public string Descrizione { get; set; }

        [StringLength(5)]
        [Column("DESCRIZIONE_BREVE")]
        public string DescrizioneBreve { get; set; }

        [Column("FATTORE_CONVERSIONE")]
        public decimal FattoreConversione { get; set; }

        [Column("FLAG_INVIO_SITRA")]
        public bool FlagInvioSitra { get; set; }
    }
}
