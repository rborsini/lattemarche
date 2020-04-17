using LatteMarche.Application.Acquirenti.Dtos;
using LatteMarche.Application.AnalisiLatte.Dtos;
using LatteMarche.Application.Destinatari.Dtos;
using LatteMarche.Application.Trasportatori.Dtos;
using LatteMarche.Core.Models;
using System.Collections.Generic;

namespace LatteMarche.WebApi.Models
{
    public class PullViewModel
    {
        public List<LaboratorioAnalisiDto> LaboratorioAnalisi { get; set; }
        public List<GiroDto> Giri { get; set; }
        public List<AcquirenteDto> Acquirenti { get; set; }
        public List<DestinatarioDto> Destinatari { get; set; }
        public List<V_Allevamento> Allevamenti { get; set; }

        public PullViewModel()
        {
            this.LaboratorioAnalisi = new List<LaboratorioAnalisiDto>();
            this.Giri = new List<GiroDto>();
            this.Acquirenti = new List<AcquirenteDto>();
            this.Destinatari = new List<DestinatarioDto>();
            this.Allevamenti = new List<V_Allevamento>();
        }

    }
}