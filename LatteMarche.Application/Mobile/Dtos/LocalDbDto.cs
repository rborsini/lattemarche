using LatteMarche.Application.Acquirenti.Dtos;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Destinatari.Dtos;
using LatteMarche.Application.Latte.Dtos;
using LatteMarche.Application.Trasportatori.Dtos;
using System.Collections.Generic;

namespace LatteMarche.Application.Mobile.Dtos
{
    public class LocalDbDto
    {
        public TrasportatoreDto Trasportatore { get; set; }

        public AutocisternaDto Autocisterna { get; set; }

        public List<GiroDto> Giri { get; set; }

        public List<AllevamentoDto> Allevamenti { get; set; }

        public List<AcquirenteDto> Acquirenti { get; set; }

        public List<DestinatarioDto> Destinatari { get; set; }

        public List<TipoLatteDto> TipiLatte { get; set; }

        public LocalDbDto()
        {
            this.Allevamenti = new List<AllevamentoDto>();
            this.Acquirenti = new List<AcquirenteDto>();
            this.Destinatari = new List<DestinatarioDto>();
            this.Giri = new List<GiroDto>();
            this.TipiLatte = new List<TipoLatteDto>();
        }

    }
}
