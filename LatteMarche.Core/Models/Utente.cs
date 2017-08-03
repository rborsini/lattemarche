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

    }
}
