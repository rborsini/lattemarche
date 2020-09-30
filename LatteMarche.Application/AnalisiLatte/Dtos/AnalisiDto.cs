using AutoMapper;
using LatteMarche.Core.Models;
using RB.Date;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.AnalisiLatte.Dtos
{
    public class AnalisiDto
    {
        public const string GRASSO = "Grasso (per calcolo)";
        public const string PROTEINE = "Proteine (per calcolo)";
        public const string CARICA_BATTERICA = "Carica Batterica Totale";
        public const string CELLULE_SOMATICHE = "Cellule somatiche";

        public string Id { get; set; }

        public string CodiceProduttore { get; set; }
        public string NomeProduttore { get; set; }
        public int? IdProduttore { get; set; }
        public int? IdAllevamento { get; set; }
        public string CodiceASL { get; set; }
        public int? TipoLatte { get; set; }
        public string TipoLatte_Descr { get; set; }
        public DateTime? DataRapportoDiProva { get; set; }
        public string DataRapportoDiProva_Str => DateHelper.FormatDate(this.DataRapportoDiProva);
        public DateTime? DataAccettazione { get; set; }
        public string DataAccettazione_Str => DateHelper.FormatDate(this.DataAccettazione);
        public DateTime? DataPrelievo { get; set; }
        public string DataPrelievo_Str => DateHelper.FormatDate(this.DataPrelievo);

        public virtual List<ValoreAnalisiDto> Valori { get; set; }

        public decimal? Grasso => GetValore(GRASSO);
        public decimal? Proteine => GetValore(PROTEINE);
        public decimal? CaricaBatterica => GetValore(CARICA_BATTERICA);
        public decimal? CelluleSomatiche => GetValore(CELLULE_SOMATICHE);

        public AnalisiDto()
        {
            this.Valori = new List<ValoreAnalisiDto>();
        }

        private decimal? GetValore(string nome)
        {
            var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            ci.NumberFormat.NumberDecimalSeparator = ",";

            var valore = this.Valori.FirstOrDefault(v => v.Nome.ToLower() == nome.ToLower());
            return valore != null && !String.IsNullOrEmpty(valore.Valore) ? Convert.ToDecimal(valore.Valore, ci) : (decimal?)null;
        }


    }



}
