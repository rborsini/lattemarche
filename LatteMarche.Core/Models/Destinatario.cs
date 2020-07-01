using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data;

namespace LatteMarche.Core.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("ANAGRAFE_DESTINATARIO")]
    public class Destinatario : Entity<int>
    {
        [Key]
        [Column("ID_DESTINATARIO")]
        public override int Id { get; set; }

        [Column("RAG_SOC_DESTINATARIO")]
        public string RagioneSociale { get; set; }

        [Column("PIVA_DESTINATARIO")]
        public string P_IVA { get; set; }

        [Column("INDIRIZZO_DESTINATARIO")]
        public string Indirizzo { get; set; }

        [ForeignKey(nameof(Comune))]
        [Column("ID_COMUNE")]
        public int? IdComune { get; set; }

        public virtual Comune Comune { get; set; }

        [Column("STABILIMENTO")]
        public string Stabilimento { get; set; }

        [Column("IDSITRA_STABILIMENTO_CASEIFICIO")]
        public int? IdSitraStabilimentoCaseificio { get; set; }

        [Column("ABILITATO")]
        public bool Abilitato { get; set; }

    }
}
