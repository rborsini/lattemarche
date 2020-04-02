using LatteMarche.Application.Assam.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Assam.Services
{
    public class ExcelParser
    {

        private static List<string> Columns = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        public static List<Report> Parse(Stream fileStream)
        {
            var reports = new List<Report>();

            HSSFWorkbook woorkbook = new HSSFWorkbook(fileStream);

            for (var i = 0; i < woorkbook.NumberOfSheets; i++)
            {
                var sheet = woorkbook.GetSheetAt(i);
                var report = ParseSheet(sheet, i);
                reports.Add(report);
            }

            return reports;
        }

        private static Report ParseSheet(ISheet sheet, int sheetIndex)
        {
            var report = new Report();

            var row = sheet.GetRow(2);
            var cell = row.GetCell(0);

            // intestazione
            report.Committente = new Cell("A3").ReadText(sheet).Replace("Committente: ", "");

            var cellaProduttoreTipoLatte = new Cell(GetCellaProduttoreRif(sheetIndex)).ReadText(sheet).Replace("Produttore:\n", "");
            var produttoreCompleto = cellaProduttoreTipoLatte.Split('\n')[0];

            report.Produttore_Codice = produttoreCompleto.Split('-')[0];
            report.Produttore_Nome = produttoreCompleto.Split('-')[1];

            var tipoLatte = cellaProduttoreTipoLatte.Split('\n')[1];

            report.TipoLatte = tipoLatte.Split(' ')[1];

            // tipi di analisi
            var tipiAnalisi = GetTipiAnalisi(sheet, sheetIndex);

            // righe analisi
            report.Analisi = GetAnalisi(sheet, sheetIndex, tipiAnalisi);

            return report;
        }

        private static string GetCellaProduttoreRif(int sheetIndex)
        {
            return sheetIndex == 0 ? "A6" : "A5";
        }

        private static string GetCellaTipiAnalisiRif(int sheetIndex)
        {
            return sheetIndex == 0 ? "H6" : "H5";
        }

        private static string GetCellaAnalisiRif(int sheetIndex)
        {
            return sheetIndex == 0 ? "A8" : "A7";
        }

        private static List<Cell> GetTipiAnalisi(ISheet sheet, int sheetIndex)
        {
            var cells = new List<Cell>();
            var previousCellIsEmpty = false;
            var currentCellIsEmpty = false;
            var cell = new Cell(GetCellaTipiAnalisiRif(sheetIndex));
            cell.ReadText(sheet);

            while (!(previousCellIsEmpty && currentCellIsEmpty))
            {
                if(!currentCellIsEmpty)
                    cells.Add(cell);

                cell = new Cell() { RowIndex = cell.RowIndex, ColIndex = cell.ColIndex + 1 };
                cell.ReadText(sheet);

                previousCellIsEmpty = currentCellIsEmpty;
                currentCellIsEmpty = String.IsNullOrEmpty(cell.TextValue);
            }

            return cells;
        }

        private static List<AnalisiLatte> GetAnalisi(ISheet sheet, int sheetIndex, List<Cell> tipiAnalisi)
        {
            var list = new List<AnalisiLatte>();

            var cell = new Cell(GetCellaAnalisiRif(sheetIndex));

            while (cell.HasBorder(sheet))
            {
                var analisi = new AnalisiLatte();

                analisi.CodiceASL = new Cell(cell.RowIndex, cell.ColIndex).ReadText(sheet);
                analisi.Campione = new Cell(cell.RowIndex, cell.ColIndex + 1).ReadText(sheet);

                analisi.DataRapportoDiProva = new Cell(cell.RowIndex, cell.ColIndex + 2).ReadDate(sheet);
                analisi.DataAccettazione = new Cell(cell.RowIndex, cell.ColIndex + 5).ReadDate(sheet);
                analisi.DataPrelievo = new Cell(cell.RowIndex, cell.ColIndex + 6).ReadDate(sheet);

                foreach (var tipoAnalisiCell in tipiAnalisi)
                {
                    var misura = new Misura();

                    misura.Nome = tipoAnalisiCell.TextValue;
                    misura.Valore = new Cell(cell.RowIndex, tipoAnalisiCell.ColIndex).ReadText(sheet);

                    if(misura.Valore.StartsWith("(#)"))
                    {
                        misura.Valore = misura.Valore.Replace("(#)", "").Trim();
                        misura.FuoriSoglia = true;
                    }
                    

                    analisi.Valori.Add(misura);
                }

                list.Add(analisi);

                cell.RowIndex++;
            }

            return list;
        }


        private class Cell
        {
            private int colIndex;
            private int rowIndex;


            public string Col
            {
                get { return ExcelParser.Columns[this.colIndex]; }
                set { this.colIndex = ExcelParser.Columns.IndexOf(value); }
            }

            public string Row
            {
                get { return $"{rowIndex + 1}"; }
                set { this.rowIndex = Convert.ToInt32(value) - 1; }
            }

            public int ColIndex
            {
                get { return this.colIndex; }
                set { this.colIndex = value; }
            }

            public int RowIndex
            {
                get { return this.rowIndex; }
                set { this.rowIndex = value; }
            }

            public string XY => $"{this.Col}{this.Row}";

            public string TextValue { get; set; }

            public DateTime? DateValue { get; set; }

            public double? NumericValue { get; set; }

            public Cell() { }

            public Cell(int rowIndex, int colIndex)
            {
                this.rowIndex = rowIndex;
                this.colIndex = colIndex;
            }

            public Cell(string cell)
            {
                this.Col = cell[0].ToString();
                this.Row = cell[1].ToString();
            }

            public string ReadText(ISheet sheet)
            {
                var text = "";

                var row = sheet.GetRow(this.rowIndex);
                if (row != null)
                {
                    var cell = row.GetCell(this.colIndex);
                    if (cell != null)
                    {
                        switch(cell.CellType)
                        {
                            case CellType.String:
                                text = cell.StringCellValue;
                                break;
                            case CellType.Numeric:
                                text = cell.NumericCellValue.ToString(CultureInfo.CreateSpecificCulture("it-IT"));
                                break;
                        }
                    }
                        
                }

                this.TextValue = text;
                return text;
            }

            public DateTime? ReadDate(ISheet sheet)
            {
                var date = (DateTime?)null;

                var row = sheet.GetRow(this.rowIndex);
                if (row != null)
                {
                    var cell = row.GetCell(this.colIndex);
                    if (cell != null)
                        date = cell.DateCellValue;
                }

                this.DateValue = date;
                return date;
            }

            public double? ReadNumeric(ISheet sheet)
            {
                var numeric = (double?)null;

                var row = sheet.GetRow(this.rowIndex);
                if (row != null)
                {
                    var cell = row.GetCell(this.colIndex);
                    if (cell != null)
                        numeric = cell.NumericCellValue;
                }

                this.NumericValue = numeric;
                return numeric;
            }

            public bool HasBorder(ISheet sheet)
            {
                var hasBorder = false;

                var row = sheet.GetRow(this.rowIndex);
                if (row != null)
                {
                    var cell = row.GetCell(this.colIndex);
                    if (cell != null)
                        hasBorder = cell.CellStyle.BorderBottom != BorderStyle.None;
                }

                return hasBorder;
            }

        }

    }
}