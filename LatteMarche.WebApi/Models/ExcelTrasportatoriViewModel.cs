using RB.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LatteMarche.WebApi.Models
{
    public class ExcelTrasportatoriViewModel
    {
        #region Constants

        private const string EXCEL_DATE_FORMAT = "dd/MM/yyyy";

        #endregion

        #region Properties

        public int Id { get; set; }

        [ExcelHeader("Data", EXCEL_DATE_FORMAT)]
        public DateTime? DataPrelievo { get; set; }

        [ExcelHeader("Lotto")]
        public string LottoConsegna { get; set; }

        [ExcelHeader("Trasportatore")]
        public string Trasportatore { get; set; }

        [ExcelHeader("Qta kg")]
        public Decimal? Quantita_Kg { get; set; }

        [ExcelHeader("Qta lt")]
        public Decimal? Quantita_Lt { get; set; }


        #endregion
    }
}