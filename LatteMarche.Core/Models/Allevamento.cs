using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatteMarche.Core.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("ANAGRAFE_ALLEVAMENTO")]
    public class Allevamento : Entity<int>
    {
        [Key]
        [Column("ID_ALLEVAMENTO")]
        public override int Id { get; set; }

        [Column("CODICE_ASL")]
        public string CodiceAsl { get; set; }

        [Column("INDIRIZZO_ALLEVAMENTO")]
        public string IndirizzoAllevamento { get; set; }

        [Column("ID_UTENTE")]
        public int? IdUtente { get; set; }

        [Column("ID_COMUNE")]
        public int IdComune { get; set; }

        public string CUAA { get; set; }
    }
}
