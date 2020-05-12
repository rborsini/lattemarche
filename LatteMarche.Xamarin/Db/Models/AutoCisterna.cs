using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LatteMarche.Xamarin.Db.Models
{
    public class AutoCisterna : AbstractEntity<int>
    {
        [Key]
        public override int Id { get; set; }

        public string Marca { get; set; }
        public string Modello { get; set; }
        public string Targa { get; set; }
        public int Portata { get; set; }
        public int NumScomparti { get; set; }

        [ForeignKey(nameof(Trasportatore))]
        public int? IdTrasportatore { get; set; }

        public Trasportatore Trasportatore { get; set; }
    }
}
