using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LatteMarche.Xamarin.Db.Models
{
    public class TemplateGiro : AbstractEntity<int>
    {
        [Key]
        public override int Id { get => base.Id; set => base.Id = value; }

        public string Codice { get; set; }

        public string Descrizione { get; set; }



        [ForeignKey(nameof(Trasportatore))]
        public int? IdTrasportatore { get; set; }

        public virtual Trasportatore Trasportatore { get; set; }

        public virtual List<Allevamento> Allevamenti { get; set; }

        public TemplateGiro()
        {
            this.Allevamenti = new List<Allevamento>();
        }

    }
}
