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
    [System.ComponentModel.DataAnnotations.Schema.Table("AZIENDA_TRASPORTATORI")]
    public class AziendaTrasportatori : Entity<int>
    {
        [Key]
        [Column("ID_AZIENDA_TRASPORTATORI")]
        public override int Id { get; set; }


        [Column("P_IVA")]
        public string P_IVA { get; set; }

        [Column("NOME_TITOLARE")]
        public string Nome { get; set; }

        [Column("COGNOME_TITOLARE")]
        public string Cognome { get; set; }

        [Column("RAGIONE_SOCIALE")]
        public string RagioneSociale { get; set; }

    }
}
