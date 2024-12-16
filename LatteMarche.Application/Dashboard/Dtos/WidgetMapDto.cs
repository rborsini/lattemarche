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
        public List<ColorLegend> Legend { get; set; }

        public WidgetMapDto() 
        {
            this.Markers = new List<Marker>();
            this.Legend = new List<ColorLegend>();
        }

    }

    public class Marker
    {
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string Color { get; set; }

        public int? Allevamento_Id { get; set; }
        public string Allevamento { get; set; }

        public int? Acquirente_Id { get; set; }
        public string Acquirente { get; set; }

        public int? TipoLatte_Id { get; set; }
        public string TipoLatte { get; set; }

        public string CodiceGiro { get; set; }
        public string Giro { get; set; }

        public int? Trasportatore_Id { get; set; }
        public string Trasportatore { get; set; }

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
