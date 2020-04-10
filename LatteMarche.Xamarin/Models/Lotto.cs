using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatteMarche.Xamarin.Models
{
    public class Lotto : Entity<string>
    {
        [Key]
        public override string Id { get; set; }

        public string Codice { get; set; }

        public virtual List<Prelievo> Prelievi { get; set; }

        public Lotto()
        {
            this.Prelievi = new List<Prelievo>();
        }


    }
}
