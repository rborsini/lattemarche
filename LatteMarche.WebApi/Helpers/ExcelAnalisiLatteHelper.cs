using ClosedXML.Excel;
using LatteMarche.Application.AnalisiLatte.Dtos;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
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

            table.Columns.Add("Codice Produttore", typeof(string));
            table.Columns.Add("Nome Produttore", typeof(string));
            table.Columns.Add("Campione", typeof(string));
            table.Columns.Add("Codice Asl", typeof(string));
            table.Columns.Add("Data rapporto di prova", typeof(DateTime));
            table.Columns.Add("Data accettazione", typeof(DateTime));
            table.Columns.Add("Data prelievo", typeof(DateTime));

            foreach(var row in list)
            {
                foreach(var value in row.Valori)
                {
                    if(!table.Columns.OfType<DataColumn>().Any(c => c.ColumnName == value.Nome))
                    {
                        table.Columns.Add(value.Nome, typeof(string));
                    }
                }
            }

            foreach(var row in list)
            {
                var dataRow = table.NewRow();

                dataRow.SetField(0, row.CodiceProduttore);
                dataRow.SetField(1, row.NomeProduttore);
                dataRow.SetField(2, row.Id);
                dataRow.SetField(3, row.CodiceASL);
                dataRow.SetField(4, row.DataRapportoDiProva);
                dataRow.SetField(5, row.DataAccettazione);
                dataRow.SetField(6, row.DataPrelievo);
                
                foreach(var value in row.Valori)
                {
                    decimal decimalValue = 0;
                    if(Decimal.TryParse(value.Valore, out decimalValue))
                        dataRow.SetField(value.Nome, decimalValue);
                    else
                        dataRow.SetField(value.Nome, value.Valore);
                }

                table.Rows.Add(dataRow);
            }

            return table;
        }

        private static byte[] ConvertToByteArray(XLWorkbook book)
        {
            var memoryStream = new MemoryStream();
            book.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }
    }
}