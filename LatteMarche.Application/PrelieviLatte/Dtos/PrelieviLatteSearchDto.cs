using RB.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.PrelieviLatte.Dtos
{
    public class PrelieviLatteSearchDto
    {

        private DateTime? dataPeriodoInizio;
        private DateTime? dataPeriodoFine;

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

        public int? IdAllevamento { get; set; }
        public int? IdTrasportatore { get; set; }
        public int? IdAcquirente { get; set; }
        public int? IdDestinatario { get; set; }
        public int? IdCessionario { get; set; }
        public int? IdTipoLatte { get; set; }

        public bool? InviatoSitra { get; set; }            

        public int IdUtente { get; set; }
             

    }
}
