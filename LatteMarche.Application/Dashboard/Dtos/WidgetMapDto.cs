using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dashboard.Dtos
{
    public class WidgetMapDto
    {
        public List<Marker> Markers { get; set; }

        public List<ColorLegend> AcquirentiLegend { get; set; }
        public List<ColorLegend> TipiLatteLegend { get; set; }

        public WidgetMapDto() 
        {
            this.Markers = new List<Marker>();
            this.AcquirentiLegend = new List<ColorLegend>();
            this.TipiLatteLegend = new List<ColorLegend>();
        }

    }

    public class Marker
    {
        public double? Lat { get; set; }
        public double? Lng { get; set; }

        public int? Allevamento_Id { get; set; }
        public string Allevamento { get; set; }


        public int? Acquirente_Id { get; set; }
        public string Acquirente { get; set; }
        public string Acquirente_Color { get; set; }


        public int? TipoLatte_Id { get; set; }
        public string TipoLatte { get; set; }
        public string TipoLatte_Color { get; set; }

        public int? UltimoPrelievo_Id { get; set; }
        
    }

    public class ColorLegend
    {
        public string Color { get; set; }
        public string Label { get; set; }

        public ColorLegend() { }
        public ColorLegend(string label, string color)
        {
            this.Label = label;
            this.Color = color;
        }

    }

}
