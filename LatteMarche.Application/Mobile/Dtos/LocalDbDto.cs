using LatteMarche.Application.Acquirenti.Dtos;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Autocisterne.Dtos;
using LatteMarche.Application.Destinatari.Dtos;
using LatteMarche.Application.Giri.Dtos;
using LatteMarche.Application.TipiLatte.Dtos;
using LatteMarche.Application.Trasportatori.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Mobile.Dtos
{
    public class LocalDbDto
    {
        public TrasportatoreDto Trasportatore { get; set; }

        public AutocisternaDto Autocisterna { get; set; }

        public GiroDto Giro { get; set; }

        public List<AllevamentoDto> Allevamenti { get; set; }

        public List<AcquirenteDto> Acquirenti { get; set; }

        public List<DestinatarioDto> Destinatari { get; set; }

        public List<TipoLatteDto> TipiLatte { get; set; }

        public LocalDbDto()
        {
            this.Allevamenti = new List<AllevamentoDto>();
            this.Acquirenti = new List<AcquirenteDto>();
            this.Destinatari = new List<DestinatarioDto>();
            this.TipiLatte = new List<TipoLatteDto>();
        }

    }
}
