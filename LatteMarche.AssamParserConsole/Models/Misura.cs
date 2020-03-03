using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.AssamParserConsole.Models
{
    public class Misura
    {
        public string Nome { get; set; }
        public string Uom { get; set; }

        public string Valore { get; set; }

        public bool FuoriSoglia { get; set; }
    }
}
