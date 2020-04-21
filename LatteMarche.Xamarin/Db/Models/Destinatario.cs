using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatteMarche.Xamarin.Db.Models
{
    public class Destinatario : AbstractEntity<int>
    {
        [Key]
        public override int Id { get => base.Id; set => base.Id = value; }

        public string RagioneSociale { get; set; }
        public string Indirizzo { get; set; }
        public string CAP { get; set; }
        public string Comune { get; set; }
        public string Provincia { get; set; }
        public string P_IVA { get; set; }
    }
}
