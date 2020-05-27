using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Rest.Dtos
{
    public class DownloadDto
    {
        public TrasportatoreDto Trasportatore { get; set; }
        public AutocisternaDto Autocisterna { get; set; }
        public List<GiroDto> Giri { get; set; }
        public List<AcquirenteDto> Acquirenti { get; set; }
        public List<DestinatarioDto> Destinatari { get; set; }
        public List<TipoLatteDto> TipiLatte { get; set; }
        public List<CessionarioDto> Cessionari { get; set; }

        public DownloadDto()
        {
            this.Acquirenti = new List<AcquirenteDto>();
            this.Destinatari = new List<DestinatarioDto>();
            this.Giri = new List<GiroDto>();
            this.TipiLatte = new List<TipoLatteDto>();
            this.Cessionari = new List<CessionarioDto>();
        }

    }
}
