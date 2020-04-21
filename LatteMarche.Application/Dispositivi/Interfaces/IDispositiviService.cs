using LatteMarche.Application.Dispositivi.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dispositivi.Interfaces
{
    public interface IDispositiviService : IEntityService<DispositivoMobile, string, DispositivoMobileDto>
    {
    }
}
