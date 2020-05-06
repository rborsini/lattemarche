using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatteMarche.Xamarin.Db.Models
{
    public class Stampante : AbstractEntity<int>
    {
        [Key]
        public override int Id { get; set; }

        public string Nome { get; set; }
        public string MacAddress { get; set; }
        public bool Selezionata { get; set; }

    }
}
