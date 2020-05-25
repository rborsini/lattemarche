using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data;

namespace LatteMarche.Core.Models
{
    [Table("UTENTE_X_CESSIONARIO")]
    public class UtenteXCessionario  : Entity<int>
    {
        [ForeignKey(nameof(Utente))]
        [Column("ID_UTENTE")]
        public override int Id { get; set; }

        public virtual Utente Utente { get; set; }

        [ForeignKey(nameof(Cessionario))]
        [Column("ID_CESSIONARIO")]
        public int IdCessionario { get; set; }

        public virtual Cessionario Cessionario { get; set; }
    }
}
