using RB.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.AnalisiLatte.Dtos
{
    public class AnalisiSearchDto
    {
        private DateTime? dataPeriodoInizio;
        private DateTime? dataPeriodoFine;

        public string Categoria { get; set; }

        public int? IdProduttore { get; set; }

        public string CodiceProduttore { get; set; }

        public int? IdAllevamento { get; set; }

        public string CodiceAsl { get; set; }

        public string Campione { get; set; }

        public DateTime? DataPeriodoInizio
        {
            get { return this.dataPeriodoInizio; }
            set { this.dataPeriodoInizio = value; }
        }

        public DateTime? DataPeriodoFine
        {
            get { return this.dataPeriodoFine; }
            set { this.dataPeriodoFine = value; }
        }

        public string DataPeriodoInizio_Str 
        { 
            get { return DateHelper.FormatDate(this.dataPeriodoInizio); } 
            set { this.dataPeriodoInizio = DateHelper.ConvertToDateTime(value); } 
        }
        public string DataPeriodoFine_Str 
        { 
            get { return DateHelper.FormatDate(this.dataPeriodoFine); } 
            set { this.dataPeriodoFine = DateHelper.ConvertToDateTime(value); } 
        }


    }
}
