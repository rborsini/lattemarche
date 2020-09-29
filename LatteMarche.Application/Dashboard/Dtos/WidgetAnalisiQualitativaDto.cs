using RB.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dashboard.Dtos
{
    public class WidgetAnalisiQualitativaDto
    {
        public class Record
        {
            public string Campione { get; set; }
            public string CodiceASL { get; set; }
            public DateTime? DataRapporto { get; set; }
            public string DataRapporto_Str => DateHelper.FormatDate(this.DataRapporto);
            public DateTime? DataAccettazione { get; set; }
            public string DataAccettazione_Str => DateHelper.FormatDate(this.DataAccettazione);
            public DateTime? DataPrelievo { get; set; }
            public string DataPrelievo_Str => DateHelper.FormatDate(this.DataPrelievo);
            public decimal? Grasso { get; set; }
            public decimal? Proteine { get; set; }
            public decimal? CaricaBatterica { get; set; }
            public decimal? CelluleSomatiche { get; set; }
        }

        public WidgetGraficoDto Grasso_Proteine { get; set; }
        public WidgetGraficoDto CaricaBatterica_CelluleSomatiche { get; set; }
        public List<Record> Records { get; set; }

        public WidgetAnalisiQualitativaDto()
        {
            this.Grasso_Proteine = new WidgetGraficoDto();
            this.CaricaBatterica_CelluleSomatiche = new WidgetGraficoDto();
            this.Records = new List<Record>();
        }

    }
}
