using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatteMarche.Xamarin.Models
{
    public class Giro : Entity<int>
    {
        [Key]
        public override int Id { get => base.Id; set => base.Id = value; }

        public string Denominazione { get; set; }

        public string CodiceGiro { get; set; }

        public int? IdTrasportatore { get; set; }

    }
}
