using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Core.Models
{
    [Table("UTENTE_X_ACQUIRENTE")]
    public class UtenteXAcquirente : Entity<int>
    {
        [Column("ID_UTENTE")]
        public override int Id { get; set; }

        [Column("ID_ACQUIRENTE")]
        public int IdAcquirente { get; set; }
    }
}
