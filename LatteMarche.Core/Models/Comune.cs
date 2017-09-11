using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatteMarche.Core.Models
{
	[System.ComponentModel.DataAnnotations.Schema.Table("COMUNI")]
    public class Comune : Entity<int>
    {
        [Key]
        [Column("ID_COMUNE")]
        public override int Id { get; set; }

        [Column("DESCRIZIONE")]
        public string Descrizione { get; set; }

        [StringLength(2)]
        [Column("PROVINCIA")]
        public string Provincia { get; set; }

        [StringLength(5)]
        [Column("CAP")]
        public string CAP { get; set; }
    }
}
