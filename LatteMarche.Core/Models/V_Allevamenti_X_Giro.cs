using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatteMarche.Core.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("V_Allevamenti_X_Giro")]
    public class V_AllevamentiXGiro : Entity<int>
    {
       // [Key]
        

        [Column("ID_GIRO")]
        public int IdGiro { get; set; }

        [Column("ID_ALLEVAMENTO")]
        public int IdAllevamento { get; set; }

        [Column("INDIRIZZO_ALLEVAMENTO")]
        public string IndirizzoAllevamento { get; set; }

        [Column("DESCRIZIONE")]
        public string Comune { get; set; }

        [Column("PROVINCIA")]
        public string Provincia { get; set; }

        [Column("NOME")]
        public string Nome { get; set; }

        [Column("COGNOME")]
        public string Cognome { get; set; }

        [Column("RAGIONE_SOCIALE")]
        public string RagioneSociale { get; set; }

        [Column("PRIORITA")]
        public int Priorita { get; set; }
    }
}
