using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Core.Models
{
    [Table("ANALISI_LATTE_VALORI")]
    public class ValoreAnalisi : Entity<long>
    {
        [Column("ID")]
        public override long Id { get; set; }

        //[Column("ID_ANALISI")]
        //public string IdAnalisi { get; set; }

        [Column("NOME")]
        public string Nome { get; set; }

        [Column("UOM")]
        public string Uom { get; set; }

        [Column("VALORE")]
        public string Valore { get; set; }

        [Column("FUORI_SOGLIA")]
        public bool FuoriSoglia { get; set; }

    }
}
