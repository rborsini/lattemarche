using System;
using AutoMapper;
using LatteMarche.Common;
using LatteMarche.Core.Models;
using RB.Date;
using RB.Excel;

namespace LatteMarche.Application.PrelieviLatte.Dtos
{
    public class PrelievoLatteDto 
    {
        private DateHelper dateHelper;

        
        public int Id { get; set; }

        public int? IdAllevamento { get; set; }
        public int? IdDestinatario { get; set; }
        public int? IdAcquirente { get; set; }
        public int? IdTrasportatore { get; set; }
        public int? IdLabAnalisi { get; set; }
        public int? IdCessionario { get; set; }
        public DateTime? DataPrelievo { get; set; }
        public DateTime? DataConsegna { get; set; }
        public DateTime? DataUltimaMungitura { get; set; }
        public Decimal? Quantita { get; set; }
        public Decimal? Temperatura { get; set; }
        public int? NumeroMungiture { get; set; }
        public string Scomparto { get; set; }
        public string LottoConsegna { get; set; }
        public string SerialeLabAnalisi { get; set; }
        public string CodiceSitra { get; set; }

        public OperationEnum LastOperation { get; set; }

        public DateTime LastChange { get; set; }

        
        public string DataPrelievoStr
        {
            get { return new DateHelper().FormatDateTime(this.DataPrelievo); }
            set { this.DataPrelievo = this.dateHelper.ConvertToDateTime(value).HasValue ? this.dateHelper.ConvertToDateTime(value).Value : DateTime.MinValue; }
        }

        
        public string OraPrelievo { get { return this.DataPrelievo.HasValue ? this.DataPrelievo.Value.ToString("HH:mm") : String.Empty; } }

        
        public string DataConsegnaStr
        {
            get { return new DateHelper().FormatDateTime(this.DataConsegna); }
            set { this.DataConsegna = this.dateHelper.ConvertToDateTime(value).HasValue ? this.dateHelper.ConvertToDateTime(value).Value : DateTime.MinValue; }
        }

        [ExcelHeader("Ora Consegna")]
        public string OraConsegna { get { return this.DataConsegna.HasValue ? this.DataConsegna.Value.ToString("HH:mm") : String.Empty; } }

        public string DataUltimaMungituraStr
        {
            get { return new DateHelper().FormatDateTime(this.DataUltimaMungitura); }
            set { this.DataUltimaMungitura = this.dateHelper.ConvertToDateTime(value).HasValue ? this.dateHelper.ConvertToDateTime(value).Value : DateTime.MinValue; }
        }

        public string OraUltimaMungitura { get { return this.DataUltimaMungitura.HasValue ? this.DataUltimaMungitura.Value.ToString("HH:mm") : String.Empty; } }

        public PrelievoLatteDto()
        {
            this.dateHelper = new DateHelper();
        }

    }

}
