using AutoMapper;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.AnalisiLatte.Dtos
{
    public class AnalisiDto
    {
        public string Id { get; set; }

        public string CodiceProduttore { get; set; }
        public string NomeProduttore { get; set; }
        public int? IdProduttore { get; set; }
        public int? IdAllevamento { get; set; }
        public string CodiceASL { get; set; }
        public int? TipoLatte { get; set; }
        public string TipoLatte_Descr { get; set; }
        public DateTime? DataRapportoDiProva { get; set; }
        public string DataRapportoDiProva_Str => ConverToString(this.DataRapportoDiProva);
        public DateTime? DataAccettazione { get; set; }
        public string DataAccettazione_Str => ConverToString(this.DataAccettazione);
        public DateTime? DataPrelievo { get; set; }
        public string DataPrelievo_Str => ConverToString(this.DataPrelievo);

        public virtual List<ValoreAnalisiDto> Valori { get; set; }

        public AnalisiDto()
        {
            this.Valori = new List<ValoreAnalisiDto>();
        }

        private string ConverToString(DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToString("dd/MM/yyyy");
            else
                return String.Empty;
        }

    }



}
