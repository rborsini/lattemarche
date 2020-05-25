using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data;

namespace LatteMarche.Core.Models
{
    [Table("UTENTE_X_DESTINATARIO")]
    public class UtenteXDestinatario : Entity<int>
    {
        [ForeignKey(nameof(Utente))]
        [Column("ID_UTENTE")]
        public override int Id { get; set; }

        public virtual Utente Utente { get; set; }

        [ForeignKey(nameof(Destinatario))]
        [Column("ID_DESTINATARIO")]
        public int IdDestinatario { get; set; }

        public virtual Destinatario Destinatario { get; set; }
    }
}
