using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Core.Models
{
    [Table("AUTORIZZAZIONI")]
    public class Autorizzazione : Entity<long>
    {
        [Key]
        public override long Id { get; set; }

        public virtual Ruolo RuoloObj { get; set; }

        [ForeignKey("RuoloObj")]
        public long IdRuolo { get; set; }

        public virtual Azione AzioneObj { get; set; }

        [ForeignKey("AzioneObj")]
        [StringLength(100)]
        public string Azione { get; set; }
    }
}
