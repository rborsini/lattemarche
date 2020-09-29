using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dashboard.Dtos
{
    public class WidgetAnalisiComparativaDto
    {
        public WidgetGraficoDto BubbleChart { get; set; }
        public WidgetGraficoDto SpiderChart { get; set; }

        public WidgetAnalisiComparativaDto()
        {
            this.BubbleChart = new WidgetGraficoDto();
            this.SpiderChart = new WidgetGraficoDto();
        }

    }
}
