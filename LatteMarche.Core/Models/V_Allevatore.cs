using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatteMarche.Core.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("V_Allevatori")]
    public class V_Allevatore : Entity<int>
    {
        [Key]
        [Column("ID_ALLEVAMENTO")]
        public override int Id { get; set; }

        [Column("CODICE_ASL")]
        public string CodiceAsl { get; set; }

        [Column("INDIRIZZO_ALLEVAMENTO")]
        public string IndirizzoAllevamento { get; set; }

        [Column("NOME")]
        public string Nome { get; set; }

        [Column("COGNOME")]
        public string Cognome { get; set; }

        [Column("RAGIONE_SOCIALE")]
        public string RagioneSociale { get; set; }

        [Column("DESCRIZIONE")]
        public string Comune { get; set; }

        [Column("PROVINCIA")]
        public string Provincia { get; set; }

        [Column("ID_UTENTE")]
        public int IdUtente { get; set; }

        [Column("ID_COMUNE")]
        public int IdComune { get; set; }

        [Column("IDSITRA_STABILIMENTO_ALLEVAMENTO")]
        public int? IdSitraStabilimentoAllevamento { get; set; }
    }
}
