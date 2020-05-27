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
    public class ExcelGiornalieriHelper
    {
        private class Record
        {
            public DateTime Data { get; set; }
            public decimal? Qta { get; set; }
        }

        public static byte[] MakeExcel(List<V_PrelievoLatte> prelievi)
        {
            HSSFWorkbook book = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)book.CreateSheet();

            // header
            MakeHeader(sheet);

            // body
            int rowIndex = 1;

            List<Record> records = new List<Record>();

            if(prelievi.Any(p => p.DataPrelievo.HasValue))
            {
                var from = prelievi.Where(p => p.DataPrelievo.HasValue).Min(p => p.DataPrelievo.Value);
                var to = prelievi.Where(p => p.DataPrelievo.HasValue).Max(p => p.DataPrelievo.Value);
                var data = new DateTime(from.Year, from.Month, from.Day);

                while(data < to)
                {
                    records.Add(new Record()
                    {
                        Data = data,
                        Qta = prelievi.Where(p => data <= p.DataPrelievo && p.DataPrelievo < data.AddDays(1)).Sum(p => p.Quantita)
                    }); 

                    data = data.AddDays(1);
                }
            }

            foreach(var record in records)
            {
                MakeDetailsRow(sheet, rowIndex, record);
                rowIndex++;
            }

            MemoryStream output = new MemoryStream();
            book.Write(output);

            return output.ToArray();
        }


        private static void MakeDetailsRow(ISheet sheet, int rowIndex, Record record)
        {
            HSSFCellStyle cellStyle = (HSSFCellStyle)sheet.Workbook.CreateCellStyle();

            // Border
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;

            cellStyle.VerticalAlignment = VerticalAlignment.Top;

            var row = sheet.CreateRow(rowIndex);

            row.CreateCell(0).SetCellValue($"{record.Data:dd-MM-yyyy}");

            if(record.Qta.HasValue)
                row.CreateCell(1).SetCellValue(Convert.ToDouble(record.Qta.Value));

            for (int i = 0; i <= 1; i++)
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

            headerRow.CreateCell(0).SetCellValue("DATA");
            headerRow.CreateCell(1).SetCellValue("QUANTITA'");

            sheet.SetColumnWidth(0, 8000);
            sheet.SetColumnWidth(1, 6000);

            for (int i = 0; i <= 1; i++)
            {
                headerRow.GetCell(i).CellStyle = headerCellStyle;
            }
        }


    }
}