using RB.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dashboard.Filters
{
    public class MapSearchDto
    {

        private DateTime? dataInizio;
        private DateTime? dataFine;

        public DateTime? DataInizio
        {
            get { return this.dataInizio; }
            set { this.dataInizio = value; }
        }

        public DateTime? DataFine
        {
            get { return this.dataFine; }
            set { this.dataFine = value; }
        }

        public string DataInizio_Str
        {
            get { return DateHelper.FormatDate(this.dataInizio); }
            set { this.dataInizio = DateHelper.ConvertToDateTime(value); }
        }

        public string DataFine_Str
        {
            get { return DateHelper.FormatDate(this.dataFine); }
            set { this.dataFine = DateHelper.ConvertToDateTime(value); }
        }

        public int? IdTipoLatte { get; set; }
        public int? IdAcquirente { get; set; }
        public string CodiceGiro { get; set; }
        public int? IdTrasportatore { get; set; }

        public string AggregazioneColore { get; set; }
        public string Tenant { get; set; }
    }
}
