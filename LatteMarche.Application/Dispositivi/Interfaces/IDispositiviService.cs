using LatteMarche.Application.Dispositivi.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Application;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.Dispositivi.Interfaces
{
    public interface IDispositiviService : IEntityService<DispositivoMobile, string, DispositivoMobileDto>
    {
        /// <summary>
        /// Ricerca dispositivi
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        PagedResult<DispositivoMobileDto> Search(DispositiviSearchDto searchDto);
    }
}
