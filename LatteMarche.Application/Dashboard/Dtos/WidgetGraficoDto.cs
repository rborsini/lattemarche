using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dashboard.Dtos
{
    public class WidgetGraficoDto
    {
        public List<SerieDto> Serie { get; set; }

        public List<string> ValoriAsseX { get; set; }

        public WidgetGraficoDto()
        {
            this.Serie = new List<SerieDto>();
            this.ValoriAsseX = new List<string>();
        }

    }



}
