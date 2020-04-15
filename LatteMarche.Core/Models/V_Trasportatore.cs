using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatteMarche.Core.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("V_Trasportatori")]
    public class V_Trasportatore : Entity<int>
    {
        [Key]
        [Column("ID_UTENTE")]
        public override int Id { get; set; }

        [Column("COGNOME")]
        public string Cognome { get; set; }

        [Column("NOME")]
        public string Nome { get; set; }

        [Column("INDIRIZZO")]
        public string Indirizzo { get; set; }

        [Column("DESCRIZIONE")]
        public string Comune { get; set; }

        [Column("PROVINCIA")]
        public string Provincia { get; set; }

        [Column("TELEFONO")]
        public string Telefono { get; set; }

        [Column("CELLULARE")]
        public string Cellulare { get; set; }

        [Column("PIVA_CF")]
        public string P_IVA { get; set; }

        [Column("RAGIONE_SOCIALE")]
        public string RagioneSociale { get; set; }
    }
}
