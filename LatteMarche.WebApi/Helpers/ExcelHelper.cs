using LatteMarche.Core.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LatteMarche.WebApi.Helpers
{
    public class ExcelHelper
    {
        public static byte[] MakeExcelTot(List<V_PrelievoLatte> prelievi)
        {
            HSSFWorkbook book = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)book.CreateSheet();

            MakeFirstRow(sheet);

            //prelievi.GroupBy(p => p.Allevamento)
            //    .Select()

            MemoryStream output = new MemoryStream();
            book.Write(output);

            return output.ToArray();
        }

        /// <summary>
        /// Riga Intestazione
        /// </summary>
        /// <param name="sheet"></param>
        private static void MakeFirstRow(ISheet sheet)
        {
            HSSFCellStyle headerCellStyle = (HSSFCellStyle)sheet.Workbook.CreateCellStyle();
            headerCellStyle.FillPattern = FillPattern.SolidForeground;
            headerCellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;

            // Border
            headerCellStyle.BorderTop = BorderStyle.Thin;
            headerCellStyle.BorderBottom = BorderStyle.Thin;
            headerCellStyle.BorderLeft = BorderStyle.Thin;
            headerCellStyle.BorderRight = BorderStyle.Thin;

            IRow headerRow = sheet.CreateRow(0);

            headerRow.CreateCell(0).SetCellValue("ALLEVATORE");
            headerRow.GetCell(0).CellStyle = headerCellStyle;

            headerRow.CreateCell(1).SetCellValue("QUANTITA' (kg)");
            headerRow.GetCell(1).CellStyle = headerCellStyle;

            headerRow.CreateCell(2).SetCellValue("QUANTITA' (lt)");
            headerRow.GetCell(2).CellStyle = headerCellStyle;

        }
    }
}