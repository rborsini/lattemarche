using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatteMarche.Core.Models
{
	[System.ComponentModel.DataAnnotations.Schema.Table("GIRO")]
    public class Giro : Entity<int>
    {
        [Key]
        [Column("ID_GIRO")]
        public override int Id { get; set; }

        [Column("DENOMINAZIONE")]
        public string Denominazione { get; set; }

        [Column("CODICE_GIRO")]
        public string CodiceGiro { get; set; }

        [Column("ID_TRASPORTATORE")]
        public int IdTrasportatore { get; set; }
    }
}
