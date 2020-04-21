using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Rest.Dtos
{
    public class DispositivoDto
    {
        public string Id { get; set; }

        public bool Attivo { get; set; }
        public DateTime DataRegistrazione { get; set; }
        public DateTime? DataUltimoDownload { get; set; }
        public DateTime? DataUltimoUpload { get; set; }

        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
        public string VersioneApp { get; set; }

        public int? IdTrasportatore { get; set; }
    }
}
