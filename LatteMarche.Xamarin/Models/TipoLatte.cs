using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatteMarche.Xamarin.Models
{
    public class TipoLatte : Entity<int>
    {
        [Key]
        public override int Id { get; set; }

        public string Codice { get; set; }

        public string Descrizione { get; set; }
    }
}
