using RB.Date;
using RB.Excel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LatteMarche.Core.Models;
using WeCode.Data;
using System.Device.Location;

namespace LatteMarche.Core.Models
{

    [System.ComponentModel.DataAnnotations.Schema.Table("V_PrelieviLatte")]
    public class V_PrelievoLatte : Entity<int>
    {
        private DateTime? dataPrelievo;
        private DateTime? dataConsegna;
        private DateTime? dataUltimaMungitura;

        [Key]
        [Column("ID_PRELIEVO")]
        public override int Id { get; set; }

        [Column("DATA_PRELIEVO")]
        public DateTime? DataPrelievo { get { return this.dataPrelievo; } set { this.dataPrelievo = value; } }

        [NotMapped]
        public string DataPrelievoStr
        {
            get { return DateHelper.FormatDate(this.dataPrelievo); }
            set { this.dataPrelievo = DateHelper.ConvertToDateTime(value); }
        }

        [Column("DATA_CONSEGNA")]
        public DateTime? DataConsegna { get { return this.dataConsegna; } set { this.dataConsegna = value; } }

        [NotMapped]
        public string DataConsegnaStr
        {
            get { return DateHelper.FormatDate(this.dataConsegna); }
            set { this.dataConsegna = DateHelper.ConvertToDateTime(value); }
        }

        [Column("SCOMPARTO")]
        public string Scomparto { get; set; }

        [Column("LOTTO_CONSEGNA")]
        public string LottoConsegna { get; set; }

        public string CodiceGiro { get { return !String.IsNullOrEmpty(this.LottoConsegna) && this.LottoConsegna.Length > 2 ? this.LottoConsegna.Substring(0, 2) : ""; } }

        [Column("QUANTITA")]
        public Decimal? Quantita { get; set; }

        [NotMapped]
        public Decimal? QuantitaLitri { get { return Fattore_Conversione.HasValue && Quantita.HasValue ? Math.Round(Quantita.Value / Fattore_Conversione.Value) : (decimal?)null; } }

        [Column("TEMPERATURA")]
        public Decimal? Temperatura { get; set; }

        [Column("DATA_ULTIMA_MUNGITURA")]
        public DateTime? DataUltimaMungitura { get { return this.dataUltimaMungitura; } set { this.dataUltimaMungitura = value; } }

        [NotMapped]
        public string DataUltimaMungituraStr
        {
            get { return DateHelper.FormatDate(this.dataUltimaMungitura); }
            set { this.dataUltimaMungitura = DateHelper.ConvertToDateTime(value); }
        }

        [Column("ID_ALLEVAMENTO")]
        public int? IdAllevamento { get; set; }

        [Column("DESCR_ALLEVAMENTO")]
        public string Allevamento { get; set; }

        [Column("PIVA_ALLEVAMENTO")]
        public string PIVA_Allevamento { get; set; }

        [Column("LAT_ALLEVAMENTO")]
        public double? Allevamento_Lat { get; set; }

        [Column("LNG_ALLEVAMENTO")]
        public double? Allevamento_Lng { get; set; }

        [Column("LAT_PRELIEVO")]
        public double? Lat { get; set; }

        [Column("LNG_PRELIEVO")]
        public double? Lng { get; set; }

        [Column("TARGA_MEZZO")]
        public string Targa { get; set; }

        public double? DistanzaAllevamento
        {
            get
            {
                if (this.Lat.HasValue && this.Lng.HasValue && this.Allevamento_Lat.HasValue && this.Allevamento_Lng.HasValue)
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


        [NotMapped]
        public string AllevamentoCompleto { get { return $"{this.Allevamento} {this.PIVA_Allevamento}"; } }

        [Column("ID_DESTINATARIO")]
        public int? IdDestinatario { get; set; }

        [Column("RAG_SOC_DESTINATARIO")]
        public string Destinatario { get; set; }

        [Column("ID_ACQUIRENTE")]
        public int? IdAcquirente { get; set; }

        [Column("RAG_SOC_ACQUIRENTE")]
        public string Acquirente { get; set; }

        [Column("ID_CESSIONARIO")]
        public int? IdCessionario { get; set; }

        [Column("RAG_SOC_CESSIONARIO")]
        public string Cessionario { get; set; }

        [Column("ID_LABANALISI")]
        public int? IdLabAnalisi { get; set; }

        [Column("ID_TRASPORTATORE")]
        public int? IdTrasportatore { get; set; }

        [Column("TRASPORTATORE")]
        [ExcelHeader("TRASPORTATORE", 4)]
        public string Trasportatore { get; set; }

        [Column("NUMERO_MUNGITURE")]
        public int? NumeroMungiture { get; set; }

        [Column("CODICE_SITRA")]
        public string CodiceSitra { get; set; }

        [Column("FATTORE_CONVERSIONE")]
        public decimal? Fattore_Conversione { get; set; }

        [Column("ID_TIPO_LATTE")]
        public int? IdTipoLatte { get; set; }

        [Column("DESCR_LATTE")]
        public string DescrizioneLatte { get; set; }

        [Column("SIGLA_LATTE")]
        public string SiglaLatte { get; set; }



    }
}
