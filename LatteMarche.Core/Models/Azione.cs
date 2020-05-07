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
    [Table("AZIONI")]
    public class Azione : Entity<string>
    {

        [Key]
        [StringLength(100)]
        public override string Id { get; set; }

        [StringLength(10)]
        public string Type { get; set; }

        [StringLength(50)]
        public string Controller { get; set; }

        [StringLength(50)]
        public string Action { get; set; }

        [StringLength(50)]
        public string ViewItem { get; set; }

        [StringLength(50)]
        public string Pagina { get; set; }

        public string Nome { get; set; }

    }
}
