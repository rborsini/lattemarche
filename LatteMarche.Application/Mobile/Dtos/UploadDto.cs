using LatteMarche.Application.Latte.Dtos;
using System.Collections.Generic;

namespace LatteMarche.Application.Mobile.Dtos
{
    public class UploadDto
    {
        public string IMEI { get; set; }

        public List<PrelievoDto> Prelievi { get; set; }

        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
        public string VersioneApp { get; set; }

    }
}
