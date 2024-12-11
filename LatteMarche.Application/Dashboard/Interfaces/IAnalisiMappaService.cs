using LatteMarche.Application.Dashboard.Dtos;
using LatteMarche.Application.Dashboard.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dashboard.Interfaces
{
    public interface IAnalisiMappaService
    {
        WidgetMapDto Load(MapSearchDto searchDto);
    }
}
