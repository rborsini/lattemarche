using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatteMarche.Xamarin.Models
{
    public class GiroItem : Entity<string>
    {
        [Key]
        public override string Id { get => base.Id; set => base.Id = value; }

        public int IdGiro { get; set; }

        public int IdAllevamento { get; set; }

        public string Allevatore { get; set; }

        public string RagioneSociale { get; set; }

        public string Indirizzo { get; set; }

        public int? Priorita { get; set; }

    }
}
