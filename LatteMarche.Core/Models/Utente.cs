using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatteMarche.Core.Models
{
	[System.ComponentModel.DataAnnotations.Schema.Table("UTENTI")]
    public class Utente : Entity<int>
    {
        [Key]
        [Column("ID_UTENTE")]
        public override int Id { get; set; }

        [Column("NOME")]
        public string Nome { get; set; }

        [Column("COGNOME")]
        public string Cognome { get; set; }

        [Column("PIVA_CF")]
        public string PivaCF { get; set; }

        [Column("INDIRIZZO")]
        public string Indirizzo { get; set; }

        [Column("LOGIN")]
        public string Username { get; set; }

        [Column("PASSWORD_NEW")]
        public string Password { get; set; }

        [Column("ID_PROFILO")]
        public int IdProfilo { get; set; }

        [Column("ABILITATO")]
        public bool Abilitato { get; set; }

        [Column("VISIBILE")]
        public bool Visibile { get; set; }

        [Column("RAGIONE_SOCIALE")]
        public string RagioneSociale { get; set; }

        [Column("CODICE_ALLEVATORE")]
        public string CodiceAllevatore { get; set; }

        [Column("QUANTITA_LATTE")]
        public int QuantitaLatte { get; set; }

        [Column("TELEFONO")]
        public string Telefono { get; set; }

        [Column("CELLULARE")]
        public string Cellulare { get; set; }

        [ForeignKey(nameof(Comune))]
        [Column("ID_COMUNE")]
        public int? IdComune { get; set; }

        public virtual Comune Comune { get; set; }

        [Column("SESSO")]
        public string Sesso { get; set; }

        [Column("ID_TIPO_LATTE")]
        public int IdTipoLatte { get; set; }

        [Column("NUMERO_COMUNICAZIONE")]
        public string NumeroComunicazione { get; set; }

        [Column("NOTE")]
        public string Note { get; set; }

        public virtual List<RuoloUtente> RuoliUtente { get; set; }
    }
}
