using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatteMarche.Xamarin.Models
{
    public class Trasportatore : Entity<int>
    {
        [Key]
        public override int Id { get; set; }
        public string TargaAutomezzo { get; set; }
        public string RagioneSociale { get; set; }
        public string Indirizzo { get; set; }
        public string P_IVA { get; set; }
    }
}
