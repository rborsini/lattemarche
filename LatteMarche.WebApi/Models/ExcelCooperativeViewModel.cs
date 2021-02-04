using RB.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LatteMarche.WebApi.Models
{
    public class ExcelCooperativeViewModel : IEquatable<ExcelCooperativeViewModel>
    {
        #region Constants

        private const string EXCEL_DATE_FORMAT = "dd/MM/yyyy";

        #endregion

        #region Properties

        [ExcelHeader("Data consegna", EXCEL_DATE_FORMAT)]
        public DateTime? DataConsegna { get; set; }

        [ExcelHeader("Lotto")]
        public string LottoConsegna { get; set; }

        [ExcelHeader("Acquirente")]
        public string Acquirente { get; set; }

        [ExcelHeader("Trasportatore")]
        public string Trasportatore { get; set; }

        [ExcelHeader("Qta kg")]
        public Decimal? Quantita_Kg { get; set; }

        [ExcelHeader("Qta lt")]
        public Decimal? Quantita_Lt { get; set; }

        public bool Equals(ExcelCooperativeViewModel other)
        {
            return this.LottoConsegna == other.LottoConsegna;
        }

        public override int GetHashCode()
        {
            return this.LottoConsegna.GetHashCode();
        }

        #endregion
    }
}