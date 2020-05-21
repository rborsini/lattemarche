using RB.Date;
using RB.Excel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LatteMarche.Core.Models;
using WeCode.Data;

namespace LatteMarche.Core.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("V_PrelieviLatte")]
    public class V_PrelievoLatte : Entity<int>
    {
        private DateHelper dateHelper;

        [Key]
        [Column("ID_PRELIEVO")]
        public override int Id { get; set; }

        [Column("DATA_PRELIEVO")]
        public DateTime? DataPrelievo { get; set; }

        [NotMapped]
        public string DataPrelievoStr
        {
            get { return new DateHelper().FormatDateTime(this.DataPrelievo); }
            set { this.DataPrelievo = this.dateHelper.ConvertToDateTime(value).HasValue ? this.dateHelper.ConvertToDateTime(value).Value : DateTime.MinValue; }
        }

        [Column("DATA_CONSEGNA")]
        public DateTime? DataConsegna { get; set; }

        [NotMapped]
        public string DataConsegnaStr
        {
            get { return new DateHelper().FormatDateTime(this.DataConsegna); }
            set { this.DataConsegna = this.dateHelper.ConvertToDateTime(value).HasValue ? this.dateHelper.ConvertToDateTime(value).Value : DateTime.MinValue; }
        }

        [Column("SCOMPARTO")]
        public string Scomparto { get; set; }

        [Column("LOTTO_CONSEGNA")]
        public string LottoConsegna { get; set; }

        [Column("QUANTITA")]
        public Decimal? Quantita { get; set; }

        [NotMapped]
        public Decimal? QuantitaLitri { get { return Fattore_Conversione.HasValue && Quantita.HasValue ? Math.Round(Quantita.Value / Fattore_Conversione.Value) : (decimal?)null; } }

        [Column("TEMPERATURA")]
        public Decimal? Temperatura { get; set; }

        [Column("DATA_ULTIMA_MUNGITURA")]
        public DateTime? DataUltimaMungitura { get; set; }

        [NotMapped]
        public string DataUltimaMungituraStr
        {
            get { return new DateHelper().FormatDateTime(this.DataUltimaMungitura); }
            set { this.DataUltimaMungitura = this.dateHelper.ConvertToDateTime(value).HasValue ? this.dateHelper.ConvertToDateTime(value).Value : DateTime.MinValue; }
        }

        [Column("ID_ALLEVAMENTO")]
        public int? IdAllevamento { get; set; }

        [Column("DESCR_ALLEVAMENTO")]
        public string Allevamento { get; set; }

        [Column("PIVA_ALLEVAMENTO")]
        public string PIVA_Allevamento { get; set; }

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

        [Column("ID_LABANALISI")]
        public int? IdLabAnalisi { get; set; }

        [Column("ID_TRASPORTATORE")]
        public int? IdTrasportatore { get; set; }

        [Column("TRASPORTATORE")]
        [ExcelHeader("TRASPORTATORE", 4)]
        public string Trasportatore { get; set; }

        [Column("TARGA_MEZZO")]
        public string Targa { get; set; }

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

        public V_PrelievoLatte()
        {
            this.dateHelper = new DateHelper();
        }
    }
}
