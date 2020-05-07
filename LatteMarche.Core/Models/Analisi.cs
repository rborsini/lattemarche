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
    [Table("ANALISI_LATTE")]
    public class Analisi : Entity<string>
    {
        [Key]
        [Column("CAMPIONE")]
        public override string Id { get; set; }

        [Column("CODICE_PRODUTTORE")]
        public string CodiceProduttore { get; set; }

        [Column("NOME_PRODUTTORE")]
        public string NomeProduttore { get; set; }

        /// <summary>
        /// Relazione tra i campi UTENTI.CODICE_ALLEVATORE e REPORT.PRODUTTORE_CODICE
        /// </summary>
        [Column("ID_PRODUTTORE")]
        public int? IdProduttore { get; set; }

        /// <summary>
        /// Relazione tra i campi CODICE_ASL dell'analisi e dell'allevamento
        /// </summary>
        [Column("ID_ALLEVAMENTO")]
        public int? IdAllevamento { get; set; }

        [Column("CODICE_ASL")]
        public string CodiceASL { get; set; }

        [Column("TIPO_LATTE")]
        public string TipoLatte_Descr { get; set; }

        [Column("ID_TIPO_LATTE")]
        public int? TipoLatte { get; set; }

        [Column("DATA_RAPPORTO_DI_PROVA")]
        public DateTime? DataRapportoDiProva { get; set; }

        [Column("DATA_ACCETTAZIONE")]
        public DateTime? DataAccettazione { get; set; }

        [Column("DATA_PRELIEVO")]
        public DateTime? DataPrelievo { get; set; }

        public virtual List<ValoreAnalisi> Valori { get; set; }

        public Analisi()
        {
            this.Valori = new List<ValoreAnalisi>();
        }

    }
}
