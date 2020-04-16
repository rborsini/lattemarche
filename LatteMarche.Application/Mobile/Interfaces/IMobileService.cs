using LatteMarche.Application.Mobile.Dtos;
using LatteMarche.Application.Trasportatori.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Mobile.Interfaces
{
    public interface IMobileService
    {
        void Register(DeviceInfoDto deviceInfo);

        LocalDbDto Download(string imei);

        void Upload(UploadDto dow);
    }
}
