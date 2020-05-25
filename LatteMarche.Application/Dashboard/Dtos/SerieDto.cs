using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dashboard.Dtos
{
    public class SerieDto
    {
        public string Nome { get; set; }
        public List<decimal> Valori { get; set; }
    }
}
