using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatteMarche.Core.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("V_Prelievi_Latte")]
    public class V_PrelievoLatte : Entity<int>
    {
        [Key]
        [Column("ID_PRELIEVO")]
        public override int Id { get; set; }

        [Column("ID_ALLEVAMENTO")]
        public int IdAllevamento { get; set; }

        [Column("DATA_PRELIEVO")]
        public DateTime DataPrelievo { get; set; }

        [Column("DATA_ULTIMA_MUNGITURA")]
        public DateTime DataUltimaMungitura { get; set; }

        [Column("NUMERO_MUNGITURE")]
        public int NumeroMungiture { get; set; }

        [Column("QUANTITA")]
        public Decimal Quantita { get; set; }

        [Column("TEMPERATURA")]
        public Decimal Temperatura { get; set; }

        [Column("ID_TRASPORTATORE")]
        public int IdTrasportatore { get; set; }

        [Column("TrasportatoreCognome")]
        public string TrasportatoreCognome { get; set; }

        [Column("ID_ACQUIRENTE")]
        public int IdAquirente { get; set; }

        [Column("AcquirenteRagSoc")]
        public string AcquirenteRagSoc { get; set; }

        [Column("ID_DESTINATARIO")]
        public int IdDestinatario { get; set; }

        [Column("DestinatarioRagSoc")]
        public string DestinatarioRagSoc { get; set; }

        [Column("DATA_CONSEGNA")]
        public DateTime DataConsegna { get; set; }

        [Column("SCOMPARTO")]
        public string Scomparto { get; set; }

        [Column("LOTTO_CONSEGNA")]
        public string LottoConsegna { get; set; }

        [Column("ID_LABANALISI")]
        public int IdLabAnalisi { get; set; }

        [Column("LabAnalisi")]
        public string LabAnalisi { get; set; }

        [Column("SERIALE_LAB_ANALISI")]
        public string SerialeLabAnalisi { get; set; }

    }
}
