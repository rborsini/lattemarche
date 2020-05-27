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
        private DateTime dataRegistratione;
        private DateTime? dataUltimoDownload;
        private DateTime? dataUltimoUpload;

        public string Id { get; set; }

        public bool Attivo { get; set; }

        public DateTime DataRegistrazione
        {
            get { return this.dataRegistratione; }
            set { this.dataRegistratione = value; }
        }

        public string DataRegistrazione_Str 
        {
            get { return new DateHelper().FormatDate(this.dataRegistratione); } 
            set { this.dataRegistratione = new DateHelper().ConvertToDateTime(value).Value; } 
        }

        public DateTime? DataUltimoDownload
        {
            get { return this.dataUltimoDownload; }
            set { this.dataUltimoDownload = value; }
        }

        public string DataUltimoDownload_Str
        {
            get { return new DateHelper().FormatDate(this.dataUltimoDownload); }
            set { this.dataUltimoDownload = new DateHelper().ConvertToDateTime(value); }
        }

        public DateTime? DataUltimoUpload
        {
            get { return this.dataUltimoUpload; }
            set { this.dataUltimoUpload = value; }
        }

        public string DataUltimoUpload_Str
        {
            get { return new DateHelper().FormatDate(this.dataUltimoUpload); }
            set { this.dataUltimoUpload = new DateHelper().ConvertToDateTime(value); }
        }

        public decimal? Latitudine { get; set; }
        public decimal? Longitudine { get; set; }
        public string VersioneApp { get; set; }

        public int? IdTrasportatore { get; set; }

        public int? IdAutocisterna { get; set; }

        public string Autocisterna_Targa { get; set; }

        public string Trasportatore_RagioneSociale { get; set; }

        public string Modello { get; set; }
        public string Marca { get; set; }
        public string VersioneOS { get; set; }
        public string Nome { get; set; }


    }
}
