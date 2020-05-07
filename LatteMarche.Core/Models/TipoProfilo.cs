using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeCode.Data;

namespace LatteMarche.Core.Models
{
	[System.ComponentModel.DataAnnotations.Schema.Table("PROFILO")]
    public class TipoProfilo : Entity<int>
    {
        [Key]
        [Column("ID_PROFILO")]
        public override int Id { get; set; }

        [Column("DESCRIZIONE_PROFILO")]
        public string Descrizione { get; set; }
        
    }
}
