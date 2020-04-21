using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Mobile.Dtos
{
    public class TipoLatteDto
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
        public string DescrizioneBreve { get; set; }
        public decimal FattoreConversione { get; set; }

    }
}
