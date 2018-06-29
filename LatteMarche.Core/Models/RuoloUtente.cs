using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Core.Models
{
    [Table("RUOLI_UTENTE")]
    public class RuoloUtente : Entity<long>
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }

        public virtual Ruolo RuoloObj { get; set; }

        [ForeignKey("RuoloObj")]
        public long? IdRuolo { get; set; }

        public virtual Utente UtenteObj { get; set; }

        [ForeignKey("UtenteObj")]
        public int Username { get; set; }
    }
}
