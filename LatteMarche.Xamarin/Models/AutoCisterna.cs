using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatteMarche.Xamarin.Models
{
    public class AutoCisterna : Entity<int>
    {
        [Key]
        public override int Id { get; set; }

        public string Marca { get; set; }

        public string Modello { get; set; }

        public string Targa { get; set; }

        public int IdTrasportatore { get; set; }

        public int Portata { get; set; }

        public int NumScomparti { get; set; }

    }
}
