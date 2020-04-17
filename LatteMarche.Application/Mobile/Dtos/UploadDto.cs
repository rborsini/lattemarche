using LatteMarche.Application.Latte.Dtos;

namespace LatteMarche.Application.Mobile.Dtos
{
    public class UploadDto
    {
        public string IMEI { get; set; }

        public LottoDto Lotto { get; set; }

        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
        public string VersioneApp { get; set; }

    }
}
