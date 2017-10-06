using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatteMarche.Core.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("ALLEVAMENTO_X_GIRO")]
    public class AllevamentoXGiro : Entity<int>
    {
   
        /*[Key]
        [Column("ID")]
        public override int Id { get; set; }*/

        [Key, Column(Order = 0, TypeName ="ID_GIRO")]
        //[Column("ID_GIRO"), Column(Order = 0)]
        public int IdGiro { get; set; }

        [Key, Column(Order = 1, TypeName = "ID_ALLEVAMENTO")]
        //[Column("ID_ALLEVAMENTO")]
        public int IdAllevamento { get; set; }

        [Column("PRIORITA")]
        public int? Priorita { get; set; }
    }
}
