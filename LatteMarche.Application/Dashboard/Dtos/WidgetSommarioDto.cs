using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dashboard.Dtos
{
    public class WidgetSommarioDto
    {
        public decimal? Qta_Settimanale { get; set; }
        public decimal? Qta_Mensile { get; set; }
        public decimal? Qta_Annuale { get; set; }
    }
}
