using LatteMarche.Application.Dashboard.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dashboard.Interfaces
{
    public interface IWidgetsService
    {
        WidgetSommarioDto WidgetSommario(int idUtente);

        WidgetGraficoDto WidgetTipiLatte(int idUtente);

        WidgetGraficoDto WidgetAcquirenti(int idUtente);
    }
}
