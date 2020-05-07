using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeCode.Data;

namespace LatteMarche.Core.Models
{
	[System.ComponentModel.DataAnnotations.Schema.Table("DOCUMENTI")]
    public class Documento : Entity<int>
    {
        [Key]
        [Column("ID_DOCUMENTO")]
        public override int Id { get; set; }

        [Column("DESCRIZIONE")]
        public string Descrizione { get; set; }

        [Column("PATH_DOCUMENTO")]
        public string PathDocumento { get; set; }

        [Column("ID_UTENTE")]
        public int IdUtente { get; set; }

        [Column("DATA_INSERIMENTO")]
        public DateTime? DataInserimento { get; set; }
    }
}
