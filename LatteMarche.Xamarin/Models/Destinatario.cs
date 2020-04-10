using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatteMarche.Xamarin.Models
{
    public class Destinatario : Entity<int>
    {
        [Key]
        public override int Id { get => base.Id; set => base.Id = value; }

        public string RagioneSociale { get; set; }
        public string Indirizzo { get; set; }
        public string CAP { get; set; }
        public string Comune { get; set; }
        public string SiglaProvincia { get; set; }
        public string P_IVA { get; set; }
    }
}
