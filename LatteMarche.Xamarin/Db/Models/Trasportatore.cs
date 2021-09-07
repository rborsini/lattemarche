using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace LatteMarche.Xamarin.Db.Models
{
    public class Trasportatore : AbstractEntity<int>
    {
        [Key]
        public override int Id { get; set; }
        public string RagioneSociale { get; set; }
        public string Indirizzo { get; set; }
        public string P_IVA { get; set; }

        public string Riga_1 => $"Targa automezzo: {this.AutoCisterna.Targa}";
        public string Riga_2 => $"Trasportatore: {this.RagioneSociale}";
        public string Riga_4 => $"P. IVA: {this.P_IVA}";

        public virtual List<AutoCisterna> AutoCisterne { get; set; }

        public AutoCisterna AutoCisterna
        {
            get
            {
                if (this.AutoCisterne.Count(a => a.Selezionata) == 1)
                    return this.AutoCisterne.FirstOrDefault(a => a.Selezionata);
                else
                    return this.AutoCisterne.FirstOrDefault();
            }            
        }

        public Trasportatore()
        {
            this.AutoCisterne = new List<AutoCisterna>();
        }


    }
}
