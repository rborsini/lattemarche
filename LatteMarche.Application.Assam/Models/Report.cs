using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Assam.Models
{
    public class Report
    {
        public string Categoria { get; set; }

        public string Committente { get; set; }

        public string Produttore_Codice { get; set; }

        public string Produttore_Nome { get; set; }

        public string TipoLatte { get; set; }

        public List<AnalisiLatte> Analisi { get; set; }

        public Report()
        {
            this.Analisi = new List<AnalisiLatte>();
        }

    }
}
