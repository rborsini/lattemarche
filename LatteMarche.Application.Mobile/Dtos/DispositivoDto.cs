using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Mobile.Dtos
{
    public class DispositivoDto
    {

        public string Id { get; set; }

        public string Modello { get; set; }
        public string Marca { get; set; }
        public string VersioneOS { get; set; }
        public string Nome { get; set; }

        public bool Attivo { get; set; }
        public DateTime DataRegistrazione { get; set; }
        public DateTime? DataUltimoDownload { get; set; }
        public DateTime? DataUltimoUpload { get; set; }

        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
        public string VersioneApp { get; set; }

        public int? IdTrasportatore { get; set; }
        public int? IdAutocisterna { get; set; }


    }
}
