using LatteMarche.Application.Mobile.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Mobile.Interfaces
{
    public interface IMobileService
    {

        DispositivoDto Register(DispositivoDto dispostivo);

        DownloadDto Download(string imei);

        void Upload(UploadDto dow);
    }
}
