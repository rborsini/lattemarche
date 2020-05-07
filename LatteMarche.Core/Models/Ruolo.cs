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
    [Table("RUOLI")]
    public class Ruolo : Entity<long>
    {

        [Key]
        public override long Id { get; set; }

        [StringLength(50)]
        [Column("Ruolo")]
        public string Codice { get; set; }

        [StringLength(50)]
        public string Descrizione { get; set; }

        public virtual List<RuoloUtente> UtentiRuolo { get; set; }

    }
}
