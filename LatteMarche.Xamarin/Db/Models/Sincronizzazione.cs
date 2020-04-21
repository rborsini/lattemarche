using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LatteMarche.Xamarin.Db.Models
{
    public class Sincronizzazione : AbstractEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string Tipo { get; set; }



    }
}
