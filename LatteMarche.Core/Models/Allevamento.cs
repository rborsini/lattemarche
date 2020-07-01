using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeCode.Data;

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

        [ForeignKey(nameof(Utente))]
        [Column("ID_UTENTE")]
        public int? IdUtente { get; set; }

        public virtual Utente Utente { get; set; }

        [ForeignKey(nameof(Comune))]
        [Column("ID_COMUNE")]
        public int? IdComune { get; set; }

        public virtual Comune Comune { get; set; }

        public string CUAA { get; set; }

        [Column("IDSITRA_STABILIMENTO_ALLEVAMENTO")]
        public int? IdSitraStabilimentoAllevamento { get; set; }

        [Column("LATITUDINE")]
        public double? Latitudine { get; set; }

        [Column("LONGITUDINE")]
        public double? Longitudine { get; set; }

        [Column("ABILITATO")]
        public bool Abilitato { get; set; }
    }
}
