using System;
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
        public string Login { get; set; }

        [Column("PASSWORD")]
        public string Password { get; set; }

        [Column("ID_PROFILO")]
        public int IdProfilo { get; set; }

        [Column("ABILITATO")]
        public bool? Abilitato { get; set; }

        [Column("VISIBILE")]
        public bool? Visibile { get; set; }

        [Column("RAGIONE_SOCIALE")]
        public string RagioneSociale { get; set; }

        [Column("CODICE_ALLEVATORE")]
        public string CodiceAllevatore { get; set; }

        [Column("QUANTITA_LATTE")]
        public int QuantitaLatte { get; set; }
    }
}
