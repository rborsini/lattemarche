using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Rest.Dtos
{
    public class TrasbordoDto
    {
        public long Id { get; set; }
        public string IMEI_Origine { get; set; }
        public string Targa_Origine { get; set; }
        public string IMEI_Destinazione { get; set; }
        public string Targa_Destinazione { get; set; }
        public DateTime Data { get; set; }
        public int IdTemplateGiro { get; set; }
        public List<PrelievoLatteDto> Prelievi { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }

        public TrasbordoDto()
        {
            this.Prelievi = new List<PrelievoLatteDto>();
        }
    }
}
