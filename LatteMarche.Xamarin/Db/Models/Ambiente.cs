using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatteMarche.Xamarin.Db.Models
{
    public class Ambiente : AbstractEntity<int>
    {
        [Key]
        public override int Id { get; set; }

        public string Nome { get; set; }

        public string Url { get; set; }

        public bool Selezionato { get; set; }

    }
}
