using LatteMarche.Application.Mobile.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Mobile.Interfaces
{
    public interface ITrasbordiService
    {
        TrasbordoDto Push(TrasbordoDto dto);

        List<TrasbordoDto> Pull(string imei);

        void Close(long id);
    }
}
