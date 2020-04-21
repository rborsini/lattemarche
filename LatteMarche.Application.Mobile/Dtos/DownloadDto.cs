using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Mobile.Dtos
{
    public class DownloadDto
    {
        public TrasportatoreDto Trasportatore { get; set; }
        public AutocisternaDto Autocisterna { get; set; }
        public List<TemplateGiroDto> Giri { get; set; }
        public List<TipoLatteDto> TipiLatte { get; set; }
        public List<AcquirenteDto> Acquirenti { get; set; }
        public List<DestinatarioDto> Destinatari { get; set; }

        public DownloadDto()
        {
            this.Giri = new List<TemplateGiroDto>();
            this.TipiLatte = new List<TipoLatteDto>();
            this.Acquirenti = new List<AcquirenteDto>();
            this.Destinatari = new List<DestinatarioDto>();
        }

    }
}
