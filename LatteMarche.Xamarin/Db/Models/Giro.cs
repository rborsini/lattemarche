using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LatteMarche.Xamarin.Db.Models
{
    public class Giro : AbstractEntity<int>
    {
        [Key]
        public override int Id { get; set; }

        public int? IdTemplateGiro { get; set; }

        public string Titolo { get; set; }

        public string CodiceLotto { get; set; }

        public DateTime DataCreazione { get; set; }

        public DateTime? DataConsegna { get; set; }

        public DateTime? DataUpload { get; set; }

        public virtual List<Prelievo> Prelievi { get; set; }

        public Giro()
        {
            this.Prelievi = new List<Prelievo>();
        }
    }
}
