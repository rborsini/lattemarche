using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Mobile.Dtos
{
    public class DeviceInfoDto
    {
        public string IMEI { get; set; }

        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }

        public string VersioneApp { get; set; }
    }
}
