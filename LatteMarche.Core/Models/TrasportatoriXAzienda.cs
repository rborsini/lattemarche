using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data;

namespace LatteMarche.Core.Models
{
    [Table("TRASPORTATORI_X_AZIENDA")]
    public class TrasportatoreXAzienda : Entity<int>
    {

        [ForeignKey(nameof(Utente))]
        [Column("ID_UTENTE")]
        public override int Id { get; set; }

        public virtual Utente Utente { get; set; }

        [Column("ID_AZIENDA_TRASPORTATORI")]
        public int IdAzienda { get; set; }

    }
}
