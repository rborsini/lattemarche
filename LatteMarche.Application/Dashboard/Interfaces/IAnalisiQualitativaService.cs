using LatteMarche.Application.Dashboard.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dashboard.Interfaces
{
    public interface IAnalisiQualitativaService
    {
        WidgetAnalisiQualitativaDto Load(int idAllevamento, DateTime from, DateTime to);
    }
}
