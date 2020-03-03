using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.AssamParserConsole.Models
{
    public class AnalisiLatte
    {
        public string CodiceASL { get; set; }

        public string Campione { get; set; }

        public DateTime? DataRapportoDiProva { get; set; }

        public DateTime? DataAccettazione { get; set; }

        public DateTime? DataPrelievo { get; set; }

        public List<Misura> Valori { get; set; }

        public AnalisiLatte()
        {
            this.Valori = new List<Misura>();
        }
    }
}
