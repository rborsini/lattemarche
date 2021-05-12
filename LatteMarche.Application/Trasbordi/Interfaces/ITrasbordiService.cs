using LatteMarche.Application.Trasbordi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Trasbordi.Interfaces
{
    public interface ITrasbordiService
    {
        List<TrasbordoDto> Search(TrasbordiSearchDto searchDto);

        TrasbordoDto Details(long id);
    }
}
