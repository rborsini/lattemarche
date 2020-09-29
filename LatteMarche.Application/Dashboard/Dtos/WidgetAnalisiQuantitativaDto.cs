using RB.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dashboard.Dtos
{

    public class WidgetAnalisiQuantitativaDto
    {
        public class Record
        {
            public int Id { get; set; } 
            public DateTime Data { get; set; }
            public string Data_Str => DateHelper.FormatDate(this.Data);
            public decimal? Qta_Kg { get; set; }
            public decimal? Qta_Lt { get; set; }
            public decimal? Temperatura { get; set; }
            public string Trasportatore { get; set; }
            public string Acquirente { get; set; }
            public string Destinatario { get; set; }
            public string TipoLatte { get; set; }
        }

        public WidgetGraficoDto AndamentoMensile { get; set; }
        public WidgetGraficoDto AndamentoGiornaliero { get; set; }
        public List<Record> Records { get; set; }

        public WidgetAnalisiQuantitativaDto()
        {
            this.AndamentoMensile = new WidgetGraficoDto();
            this.AndamentoGiornaliero = new WidgetGraficoDto();
            this.Records = new List<Record>();
        }

    }
}
