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
    [Table("Logs")]
    public class LogRecord : Entity<long>
    {

        [Key]
        public override long Id { get; set; }

        public DateTime Date { get; set; }

        public string Thread { get; set; }

        public string Level { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }

        public string Source { get; set; }

        public string HostName { get; set; }

        public string Identity { get; set; }

        public decimal Duration { get; set; }

        public string Request { get; set; }

        public string Arguments { get; set; }
    }
}
