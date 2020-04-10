using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatteMarche.Xamarin.Models
{
    public class Allevamento : Entity<int>
    {
        [Key]
        public override int Id { get; set; }
        public string CodiceAsl { get; set; }
        public string IndirizzoAllevamento { get; set; }
        public int? IdUtente { get; set; }
        public string SiglaProvincia { get; set; }
        public int IdComune { get; set; }
        public string CUAA { get; set; }

        public string Prov { get; set; }
        public string RagioneSociale { get; set; }
        public string P_IVA { get; set; }

        public string CAP { get; set; }

        public string Comune { get; set; }
    }
}
