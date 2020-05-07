using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeCode.Data;

namespace LatteMarche.Core.Models
{
	[System.ComponentModel.DataAnnotations.Schema.Table("ANAGRAFE_ACQUIRENTE")]
    public class Acquirente : Entity<int>
    {
        [Key]
        [Column("ID_ACQUIRENTE")]
        public override int Id { get; set; }

        [Column("RAG_SOC_ACQUIRENTE")]
        public string RagioneSociale { get; set; }

        [Column("PIVA_ACQUIRENTE")]
        public string Piva { get; set; }

        [Column("INDIRIZZO_ACQUIRENTE")]
        public string Indirizzo { get; set; }

        [ForeignKey(nameof(Comune))]
        [Column("ID_COMUNE")]
        public int? IdComune { get; set; }

        public virtual Comune Comune { get; set; }

        [Column("IDSITRA_ACQUIRENTE")]
        public int? IdSitra { get; set; }
    }
}
