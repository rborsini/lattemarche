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
    [Table("TRASBORDI")]
    public class Trasbordo : Entity<long>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }
        public string IMEI_Origine { get; set; }
        public string Targa_Origine { get; set; }
        public string IMEI_Destinazione { get; set; }
        public string Targa_Destinazione { get; set; }
        public DateTime Data { get; set; }
        public int IdTemplateGiro { get; set; }
        public string Prelievi_JSON { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
        public bool Chiuso { get; set; }
    }
}
