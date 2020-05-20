using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data;

namespace LatteMarche.Core.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("DISPOSITIVI_MOBILI")]
    public class DispositivoMobile : Entity<string>
    {
        [Key]
        [Column("IMEI")]
        public override string Id { get; set; }

        [Column("MODELLO")]
        public string Modello { get; set; }

        [Column("MARCA")]
        public string Marca { get; set; }

        [Column("VERSIONE_OS")]
        public string VersioneOS { get; set; }

        [Column("NOME")]
        public string Nome { get; set; }

        [Column("ATTIVO")]
        public bool Attivo { get; set; }

        [Column("DATA_REGISTRAZIONE")]
        public DateTime DataRegistrazione { get; set; }

        [Column("DATA_ULTIMO_DOWNLOAD")]
        public DateTime? DataUltimoDownload { get; set; }

        [Column("DATA_ULTIMO_UPLOAD")]
        public DateTime? DataUltimoUpload { get; set; }

        [Column("VERSIONE_APP")]
        public string VersioneApp { get; set; }

        [Column("LATITUDINE")]
        public decimal? Latitudine { get; set; }

        [Column("LONGITUDINE")]
        public decimal? Longitudine { get; set; }

        [ForeignKey(nameof(Trasportatore))]
        [Column("ID_TRASPORTATORE")]
        public int? IdTrasportatore { get; set; }

        public virtual Utente Trasportatore { get; set; }

        [ForeignKey(nameof(Autocisterna))]
        [Column("ID_AUTOCISTERNA")]
        public int? IdAutocisterna { get; set; }

        public virtual Autocisterna Autocisterna { get; set; }

    }
}
