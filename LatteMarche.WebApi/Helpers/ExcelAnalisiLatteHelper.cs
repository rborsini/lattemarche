using ClosedXML.Excel;
using LatteMarche.Application.AnalisiLatte.Dtos;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace LatteMarche.WebApi.Helpers
{
    public class ExcelAnalisiLatteHelper
    {
        public static byte[] MakeExcel(List<AnalisiDto> analisi)
        {
            var book = new XLWorkbook();

            var dataTable = ConvertToDataTable(analisi);

            book.Worksheets.Add(dataTable);

            return ConvertToByteArray(book);
        }

        private static DataTable ConvertToDataTable(List<AnalisiDto> list)
        {
            var table = new DataTable();

            table.TableName = "Analisi latte";

            table.Columns.Add("Categoria", typeof(string));
            table.Columns.Add("Codice Produttore", typeof(string));
            table.Columns.Add("Nome Produttore", typeof(string));
            table.Columns.Add("Campione", typeof(string));
            table.Columns.Add("Codice Asl", typeof(string));
            table.Columns.Add("Data rapporto di prova", typeof(DateTime));
            table.Columns.Add("Data accettazione", typeof(DateTime));
            table.Columns.Add("Data prelievo", typeof(DateTime));

            var formatProvider = new CultureInfo("it-IT");

            foreach(var row in list)
            {
                var i = 0;
                foreach(var value in row.Valori)
                {
                    if(!table.Columns.OfType<DataColumn>().Any(c => c.ColumnName == value.Nome))
                    {
                        var type = GetColumnType(list, i);
                        table.Columns.Add(value.Nome, type);
                    }
                    i++;
                }
            }

            foreach(var row in list)
            {
                var dataRow = table.NewRow();

                dataRow.SetField(0, row.Categoria);
                dataRow.SetField(1, row.CodiceProduttore);
                dataRow.SetField(2, row.NomeProduttore);
                dataRow.SetField(3, row.Id);
                dataRow.SetField(4, row.CodiceASL);
                dataRow.SetField(5, row.DataRapportoDiProva);
                dataRow.SetField(6, row.DataAccettazione);
                dataRow.SetField(7, row.DataPrelievo);
                
                foreach(var value in row.Valori)
                {
                    decimal decimalValue = 0;
                    if(Decimal.TryParse(value.Valore, System.Globalization.NumberStyles.Float, formatProvider, out decimalValue))
                        dataRow.SetField(value.Nome, decimalValue);
                    else
                        dataRow.SetField(value.Nome, value.Valore);
                }

                table.Rows.Add(dataRow);
            }

            return table;
        }

        private static Type GetColumnType(List<AnalisiDto> list, int columnIndex)
        {
            var cellValues = list.Select(r => columnIndex < r.Valori.Count ? r.Valori[columnIndex].Valore : "").ToList();
            decimal decimalValue = 0;
            if (cellValues.All(v => Decimal.TryParse(v, out decimalValue)))
                return typeof(decimal);
            else
                return typeof(string);
        }

        private static byte[] ConvertToByteArray(XLWorkbook book)
        {
            var memoryStream = new MemoryStream();
            book.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }
    }
}