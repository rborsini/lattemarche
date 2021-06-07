using RB.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Trasbordi.Dtos
{
    public class TrasbordoDto
    {
        public long Id { get; set; }

        public string Targa_Origine { get; set; }        
        public string Targa_Destinazione { get; set; }

        public int IdTemplateGiro { get; set; }
        public string DenominazioneGiro { get; set; }

        public double? Lat { get; set; }
        public double? Lng { get; set; }

        public DateTime Data { get; set; }

        public string Data_Str => DateHelper.FormatDateTime(this.Data);
    }
}
