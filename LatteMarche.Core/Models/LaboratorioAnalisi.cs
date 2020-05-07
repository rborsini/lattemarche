using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data;

namespace LatteMarche.Core.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("LaboratoriAnalisi")]
    public class LaboratorioAnalisi : Entity<int>
    {
        [Key]
        public override int Id { get; set;  }

        public string Descrizione { get; set; }

    }
}
