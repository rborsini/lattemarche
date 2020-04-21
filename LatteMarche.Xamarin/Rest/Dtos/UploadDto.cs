using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Dtos
{
    public class UploadDto
    {
        public string IMEI { get; set; }

        public List<PrelievoLatteDto> Prelievi { get; set; }

        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
        public string VersioneApp { get; set; }

    }
}
