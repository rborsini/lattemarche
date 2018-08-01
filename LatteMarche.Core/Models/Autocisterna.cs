using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatteMarche.Core.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("AUTOCISTERNA")]
    public class Autocisterna : Entity<int>
    {

        [Key]
        [Column("ID_VEICOLO")]
        public override int Id { get; set; }

        [Column("MARCA")]
        public string Marca { get; set; }

        [Column("MODELLO")]
        public string Modello { get; set; }

        [Column("TARGA_MEZZO")]
        public string Targa { get; set; }

        [Column("ID_TRASPORTATORE")]
        public int? IdTrasportatore { get; set; }

        [Column("PORTATA")]
        public int? Portata { get; set; }

        [Column("NUMERO_SCOMPARTI")]
        public int? NumScomparti { get; set; }

    }
}
