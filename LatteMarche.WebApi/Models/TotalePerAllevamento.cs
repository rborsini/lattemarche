using LatteMarche.Core.Models;
using RB.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LatteMarche.WebApi.Models
{
    public class TotalePerAllevamento
    {
        public string Allevamento { get; set; }

        public List<V_PrelievoLatte> Prelievi { get; set; }

        public decimal TotaleKg { get; set; }

        public decimal TotaleLitri { get; set; }
    }
}