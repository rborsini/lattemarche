using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatteMarche.Xamarin.Db.Models
{
    public class Trasportatore : AbstractEntity<int>
    {
        [Key]
        public override int Id { get; set; }        
        public string RagioneSociale { get; set; }
        public string Indirizzo { get; set; }
        public string P_IVA { get; set; }

        public virtual AutoCisterna AutoCisterna { get; set; }


    }
}
