using System;
using System.Device.Location;
using AutoMapper;
using LatteMarche.Common;
using LatteMarche.Core.Models;
using RB.Date;
using RB.Excel;

namespace LatteMarche.Application.PrelieviLatte.Dtos
{
    public class PrelievoLatteDto 
    {
        #region Fields

        private DateTime? dataPrelievo;
        private DateTime? dataConsegna;
        private DateTime? dataUltimaMungitura;

        #endregion

        #region Properties

        public int Id { get; set; }

        public int? IdAllevamento { get; set; }
        public int? IdDestinatario { get; set; }
        public int? IdAcquirente { get; set; }
        public int? IdTrasportatore { get; set; }
        public int? IdLabAnalisi { get; set; }
        public int? IdCessionario { get; set; }
        public DateTime? DataPrelievo { get { return this.dataPrelievo; } set { this.dataPrelievo = value; } }
        public string DataPrelievoStr
        {
            get { return DateHelper.FormatDateTime(this.dataPrelievo); }
            set { this.dataPrelievo = DateHelper.ConvertToDateTime(value); }
        }
        public string OraPrelievo { get { return this.DataPrelievo.HasValue ? this.DataPrelievo.Value.ToString("HH:mm") : String.Empty; } }

        public DateTime? DataConsegna { get { return this.dataConsegna; } set { this.dataConsegna = value; } }
        public string DataConsegnaStr
        {
            get { return DateHelper.FormatDateTime(this.dataConsegna); }
            set { this.dataConsegna = DateHelper.ConvertToDateTime(value); }
        }
        public string OraConsegna { get { return this.DataConsegna.HasValue ? this.DataConsegna.Value.ToString("HH:mm") : String.Empty; } }

        public DateTime? DataUltimaMungitura { get { return this.dataUltimaMungitura; } set { this.dataUltimaMungitura = value; } }
        public string DataUltimaMungituraStr
        {
            get { return DateHelper.FormatDateTime(this.dataUltimaMungitura); }
            set { this.dataUltimaMungitura = DateHelper.ConvertToDateTime(value); }
        }

        public string OraUltimaMungitura { get { return this.DataUltimaMungitura.HasValue ? this.DataUltimaMungitura.Value.ToString("HH:mm") : String.Empty; } }

        public Decimal? Quantita { get; set; }
        public Decimal? Temperatura { get; set; }
        public int? NumeroMungiture { get; set; }
        public string Scomparto { get; set; }
        public string LottoConsegna { get; set; }
        public string SerialeLabAnalisi { get; set; }
        public string CodiceSitra { get; set; }

        public OperationEnum LastOperation { get; set; }

        public DateTime LastChange { get; set; }

        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public double? Allevamento_Lat { get; set; }
        public double? Allevamento_Lng { get; set; }

        public double? DistanzaAllevamento
        {
            get
            {
                if (this.Lat.HasValue && this.Lng.HasValue && 
                    this.Lat.Value != 0 && this.Lng.Value != 0 &&
                    this.Allevamento_Lat.HasValue && this.Allevamento_Lng.HasValue
                   )
                {
                    var coordinatePrelievo = new GeoCoordinate(this.Lat.Value, this.Lng.Value);
                    var coordinateAllevamento = new GeoCoordinate(this.Allevamento_Lat.Value, this.Allevamento_Lng.Value);

                    return coordinatePrelievo.GetDistanceTo(coordinateAllevamento);
                }
                else
                    return (double?)null;
            }
        }

        public string DistanzaAllevamento_Str { get { return this.DistanzaAllevamento.HasValue ? $"{this.DistanzaAllevamento:#0} m" : "-"; } }


        public int? IdAutocisterna { get; set; }
        public string DeviceId { get; set; }

        #endregion


    }

}
