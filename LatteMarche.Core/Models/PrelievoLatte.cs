using LatteMarche.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatteMarche.Core.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("PRELIEVO_LATTE")]
    public class PrelievoLatte : Entity<int>
    {
        [Key]
        [Column("ID_PRELIEVO")]
        public override int Id { get; set; }

        [Column("ID_ALLEVAMENTO")]
        public int IdAllevamento { get; set; }

        [Column("ID_DESTINATARIO")]
        public int IdDestinatario { get; set; }

        [Column("ID_ACQUIRENTE")]
        public int IdAquirente { get; set; }

        [Column("ID_TRASPORTATORE")]
        public int IdTrasportatore { get; set; }

        [Column("ID_LABANALISI")]
        public int IdLabAnalisi { get; set; }

        [Column("DATA_PRELIEVO")]
        public DateTime DataPrelievo { get; set; }

        [Column("DATA_CONSEGNA")]
        public DateTime DataConsegna { get; set; }

        [Column("DATA_ULTIMA_MUNGITURA")]
        public DateTime DataUltimaMungitura { get; set; }

        [Column("QUANTITA")]
        public Decimal Quantita { get; set; }

        [Column("TEMPERATURA")]
        public Decimal Temperatura { get; set; }

        [Column("NUMERO_MUNGITURE")]
        public int NumeroMungiture { get; set; }

        [Column("SCOMPARTO")]
        public string Scomparto { get; set; }

        [Column("LOTTO_CONSEGNA")]
        public string LottoConsegna { get; set; }

        [Column("SERIALE_LAB_ANALISI")]
        public string SerialeLabAnalisi { get; set; }

        public OperationEnum LastOperation { get; set; }

        public DateTime LastChange { get; set; }

    }
}
