using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data;

namespace LatteMarche.Core.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("LOTTI")]
    public class Lotto : Entity<long>
    {

        [Key]
        [Column("ID_LOTTO")]
        public override long Id { get; set; }

        [Column("CODICE")]
        public string Codice { get; set; }

        [Column("CODICE_SITRA")]
        public string CodiceSitra { get; set; }

        [Column("TS")]
        public DateTime TimeStamp { get; set; }

        [Column("INVIATO")]
        public bool Inviato { get; set; }

        [Column("ERRORE")]
        public bool Errore { get; set; }

        [Column("MESSAGGIO")]
        public string Messaggio { get; set; }

    }

}
