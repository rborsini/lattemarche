using RB.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LatteMarche.WebApi.Models
{
    public class TotaliPerAllevamento
    {
        [ExcelHeader("ALLEVAMENTO", 0)]
        public string Allevamento { get; set; }

        [ExcelHeader("QUANTITA' (kg)", 1)]
        public decimal Kg { get; set; }

        [ExcelHeader("QUANTITA' (lt)", 2)]
        public decimal Litri { get; set; }
    }
}