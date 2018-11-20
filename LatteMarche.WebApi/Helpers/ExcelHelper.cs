using LatteMarche.Core.Models;
using LatteMarche.WebApi.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using RB.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LatteMarche.WebApi.Helpers
{
    public class ExcelHelper
    {
        public static byte[] MakeExcelTot(List<V_PrelievoLatte> prelievi)
        {
            HSSFWorkbook book = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)book.CreateSheet();

            // header
            MakeHeader(sheet);

            // body
            int rowIndex = 1;

            // raggruppamento
            List<TotalePerAllevamento> groups = prelievi
                .GroupBy(p => p.Allevamento)
                .Select(p => new TotalePerAllevamento()
                {
                    Allevamento = p.Key,
                    Prelievi = p.ToList(),
                    TotaleKg = p.Where(r => r.Quantita.HasValue).Sum(r => r.Quantita.Value),
                    TotaleLitri = p.Where(r => r.QuantitaLitri.HasValue).Sum(r => r.QuantitaLitri.Value)
                })
                .OrderBy(t => t.Allevamento)
                .ToList();

            // ciclo gruppi
            foreach(var group in groups)
            {
                // ciclo prelievi
                foreach(var prelievo in group.Prelievi)
                {
                    MakeDetailsRow(sheet, rowIndex, prelievo);
                    rowIndex++;
                }

                MakeGroupRow(sheet, rowIndex, group);
                rowIndex++;
            }

            MemoryStream output = new MemoryStream();
            book.Write(output);

            return output.ToArray();
        }

        private static void MakeGroupRow(ISheet sheet, int rowIndex, TotalePerAllevamento group)
        {
            HSSFCellStyle groupCellStyle = (HSSFCellStyle)sheet.Workbook.CreateCellStyle();
            groupCellStyle.FillPattern = FillPattern.SolidForeground;
            groupCellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;

            IFont font = sheet.Workbook.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            groupCellStyle.SetFont(font);

            // Border
            groupCellStyle.BorderTop = BorderStyle.Thin;
            groupCellStyle.BorderBottom = BorderStyle.Thin;
            groupCellStyle.BorderLeft = BorderStyle.Thin;
            groupCellStyle.BorderRight = BorderStyle.Thin;

            groupCellStyle.Alignment = HorizontalAlignment.Right;
            groupCellStyle.VerticalAlignment = VerticalAlignment.Top;

            var row = sheet.CreateRow(rowIndex);

            // merge allevatore
            sheet.AddMergedRegion(new CellRangeAddress(rowIndex - group.Prelievi.Count, rowIndex - 1, 0, 0));

            // TOT
            row.CreateCell(0).SetCellValue("TOT");
            row.GetCell(0).CellStyle = groupCellStyle;

            sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 0, 7));

            // Kg
            row.CreateCell(8).SetCellFormula($"sum(I{rowIndex-group.Prelievi.Count+1}:I{rowIndex})");
            row.GetCell(8).CellStyle = groupCellStyle;

            // Litri 
            row.CreateCell(9).SetCellFormula($"sum(J{rowIndex - group.Prelievi.Count + 1}:J{rowIndex})");
            row.GetCell(9).CellStyle = groupCellStyle;

        }

        private static void MakeDetailsRow(ISheet sheet, int rowIndex, V_PrelievoLatte prelievo)
        {
            HSSFCellStyle cellStyle = (HSSFCellStyle)sheet.Workbook.CreateCellStyle();

            // Border
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;

            cellStyle.VerticalAlignment = VerticalAlignment.Top;

            var row = sheet.CreateRow(rowIndex);

            row.CreateCell(0).SetCellValue(prelievo.AllevamentoCompleto);
            row.CreateCell(1).SetCellValue(prelievo.Acquirente);
            row.CreateCell(2).SetCellValue(prelievo.Destinatario);
            row.CreateCell(3).SetCellValue(prelievo.Trasportatore);
            row.CreateCell(4).SetCellValue(prelievo.Targa);
            row.CreateCell(5).SetCellValue(prelievo.DataPrelievoStr);
            row.CreateCell(6).SetCellValue(Convert.ToDouble(prelievo.Scomparto));
            row.CreateCell(7).SetCellValue(prelievo.LottoConsegna);
            row.CreateCell(8).SetCellValue(Convert.ToDouble(prelievo.Quantita));
            row.CreateCell(9).SetCellValue(Convert.ToDouble(prelievo.QuantitaLitri));

            for (int i = 0; i <= 9; i++)
            {
                row.GetCell(i).CellStyle = cellStyle;
            }
        }

        private static void MakeHeader(ISheet sheet)
        {
            HSSFCellStyle headerCellStyle = (HSSFCellStyle)sheet.Workbook.CreateCellStyle();
            headerCellStyle.FillPattern = FillPattern.SolidForeground;
            headerCellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey50Percent.Index;

            headerCellStyle.VerticalAlignment = VerticalAlignment.Top;

            IFont font = sheet.Workbook.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.Color = NPOI.HSSF.Util.HSSFColor.White.Index;

            headerCellStyle.SetFont(font);

            // Border
            headerCellStyle.BorderTop = BorderStyle.Thin;
            headerCellStyle.BorderBottom = BorderStyle.Thin;
            headerCellStyle.BorderLeft = BorderStyle.Thin;
            headerCellStyle.BorderRight = BorderStyle.Thin;

            IRow headerRow = sheet.CreateRow(0);

            headerRow.CreateCell(0).SetCellValue("PRODUTTORE");
            headerRow.CreateCell(1).SetCellValue("ACQUIRENTE");
            headerRow.CreateCell(2).SetCellValue("DESTINATARIO");
            headerRow.CreateCell(3).SetCellValue("TRASPORTATORE");
            headerRow.CreateCell(4).SetCellValue("TARGA");
            headerRow.CreateCell(5).SetCellValue("DATA E ORA PRELIEVO");
            headerRow.CreateCell(6).SetCellValue("SCOMPARTO");
            headerRow.CreateCell(7).SetCellValue("LOTTO CONSEGNA");
            headerRow.CreateCell(8).SetCellValue("QTA (kg)");
            headerRow.CreateCell(9).SetCellValue("QTA (lt)");

            sheet.SetColumnWidth(0, 8000);
            sheet.SetColumnWidth(1, 6000);
            sheet.SetColumnWidth(2, 6000);
            sheet.SetColumnWidth(3, 7000);
            sheet.SetColumnWidth(4, 3000);
            sheet.SetColumnWidth(5, 4500);
            sheet.SetColumnWidth(6, 2000);
            sheet.SetColumnWidth(7, 4000);
            sheet.SetColumnWidth(8, 3000);
            sheet.SetColumnWidth(9, 3000);

            for (int i = 0; i <= 9; i++)
            {
                headerRow.GetCell(i).CellStyle = headerCellStyle;
            }
        }

        private static List<PropertyInfo> GetHeaderProperties(Type type)
        {
            // Recupero tutte le proprietà annotate come Header            
            List<MemberInfo> headerMembers = type.GetMembers().Where(m => m is PropertyInfo).Where(p => p.GetCustomAttribute<ExcelHeaderAttribute>(false) != null).ToList();
            IEnumerable<PropertyInfo> headerProperties = headerMembers.Select(h => h as PropertyInfo).OrderBy(h => h.GetCustomAttribute<ExcelHeaderAttribute>().Index);
            return headerProperties.ToList();
        }

        private static PropertyInfo GetPropertyInfoByHeaderName(List<PropertyInfo> properties, string headerName)
        {
            foreach (PropertyInfo pi in properties)
            {
                List<ExcelHeaderAttribute> headers = pi.GetCustomAttributes<ExcelHeaderAttribute>().ToList();

                if (headers.Select(h => h.Name).Contains(headerName))
                {
                    return pi;
                }
            }

            return null;
        }

    }
}