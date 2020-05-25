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
    public class Cessionario : Entity<int>
    {
        [Key]
        [Column("ID_CESSIONARIO")]
        public override int Id { get; set; }

        [Column("RAG_SOC_CESSIONARIO")]
        public string RagioneSociale { get; set; }

        [Column("PIVA_CESSIONARIO")]
        public string Piva { get; set; }

        [Column("INDIRIZZO_CESSIONARIO")]
        public string Indirizzo { get; set; }

        [ForeignKey(nameof(Comune))]
        [Column("ID_COMUNE")]
        public int? IdComune { get; set; }

        public virtual Comune Comune { get; set; }

    }
}
