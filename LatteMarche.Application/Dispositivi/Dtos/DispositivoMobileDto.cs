using RB.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dispositivi.Dtos
{
    public class DispositivoMobileDto 
    {
        public string Id { get; set; }

        public bool Attivo { get; set; }
        public DateTime DataRegistrazione { get; set; }

        public DateTime? DataUltimoDownload { get; set; }

        public DateTime? DataUltimoUpload { get; set; }

        public decimal? Latitudine { get; set; }
        public decimal? Longitudine { get; set; }
        public string VersioneApp { get; set; }

        public int? IdTrasportatore { get; set; }

        public string Trasportatore_RagioneSociale { get; set; }

    }
}
