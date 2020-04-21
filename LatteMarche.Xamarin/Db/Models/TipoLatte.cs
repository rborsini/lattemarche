using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatteMarche.Xamarin.Db.Models
{
    public class TipoLatte : AbstractEntity<int>
    {
        [Key]
        public override int Id { get; set; }

        public string Codice { get; set; }

        public string Descrizione { get; set; }

        public decimal? FattoreConversione { get; set; }
    }
}
