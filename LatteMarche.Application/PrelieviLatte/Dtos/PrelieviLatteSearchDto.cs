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
        #region Fields

        private DateTime? dataPeriodoInizio;
        private DateTime? dataPeriodoFine;

        private DateTime? dataConsegnaInizio;
        private DateTime? dataConsegnaFine;

        #endregion

        #region Properties

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


        public DateTime? DataConsegnaInizio
        {
            get { return this.dataConsegnaInizio; }
            set { this.dataConsegnaInizio = value; }
        }

        public DateTime? DataConsegnaFine
        {
            get { return this.dataConsegnaFine; }
            set { this.dataConsegnaFine = value; }
        }

        public string DataConsegnaInizio_Str
        {
            get { return DateHelper.FormatDate(this.dataConsegnaInizio); }
            set { this.dataConsegnaInizio = DateHelper.ConvertToDateTime(value); }
        }

        public string DataConsegnaFine_Str
        {
            get { return DateHelper.FormatDate(this.dataConsegnaFine); }
            set { this.dataConsegnaFine = DateHelper.ConvertToDateTime(value); }
        }

        public int? IdAllevamento { get; set; }
        public int? IdTrasportatore { get; set; }
        public int? IdAcquirente { get; set; }
        public int? IdDestinatario { get; set; }
        public int? IdCessionario { get; set; }
        public int? IdTipoLatte { get; set; }

        public bool? InviatoSitra { get; set; }            

        public string LottoConsegna { get; set; }

        public string CodiceGiro { get; set; }

        public int IdUtente { get; set; }

        #endregion
    }
}
