using AutoMapper;
using LatteMarche.Application.AnalisiLatte.Dtos;
using LatteMarche.Application.Dashboard.Dtos;
using LatteMarche.Application.Dashboard.Interfaces;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Dashboard.Services
{
    public class AnalisiComparativaService : IAnalisiComparativaService
    {

        #region Fields

        private IMapper mapper;

        private IRepository<Analisi, string> analisiRepository;
        private IRepository<PrelievoLatte, int> prelieviRepository;

        #endregion

        #region Constructor

        public AnalisiComparativaService(IUnitOfWork uow, IMapper mapper)
        {
            this.mapper = mapper;

            this.analisiRepository = uow.Get<Analisi, string>();
            this.prelieviRepository = uow.Get<PrelievoLatte, int>();
        }

        #endregion

        #region Methods

        public WidgetAnalisiComparativaDto Load(int idAllevamento, DateTime from, DateTime to)
        {
            var dto = new WidgetAnalisiComparativaDto();

            var analisi = GetAnalisi(from, to);

            dto.BubbleChart = MakeBubbleChart(analisi, idAllevamento, from, to);
            dto.SpiderChart = MakeSpiderChart(analisi, idAllevamento);

            return dto;
        }



        private WidgetGraficoDto MakeBubbleChart(List<AnalisiDto> analisi, int idAllevamento, DateTime from, DateTime to)
        {
            var dto = new WidgetGraficoDto();
            var serie = new SerieDto();

            var idAllevamenti = analisi.Select(a => a.IdAllevamento).Distinct().ToList();

            foreach(var id in idAllevamenti)
            {
                var analisiAllevamento = analisi.Where(a => a.IdAllevamento == id).ToList();
                var prelieviAllevamento = this.prelieviRepository.DbSet.Where(p => p.IdAllevamento == id && from <= p.DataPrelievo && p.DataPrelievo < to);
                var valoriGrasso = analisiAllevamento.Where(a => a.Grasso.HasValue).Select(a => a.Grasso);
                var valoriProteine = analisiAllevamento.Where(a => a.Proteine.HasValue).Select(a => a.Proteine);

                serie.Bolle.Add(new BollaDto()
                {
                    Nome = analisiAllevamento[0].NomeProduttore,
                    Colore = idAllevamento == id ? "#ff0000" : "",
                    X = valoriGrasso.Count() > 0 ? valoriGrasso.Average() : (decimal?)null,
                    Y = valoriProteine.Count() > 0 ? valoriProteine.Average() : (decimal?)null,
                    Z = prelieviAllevamento.Count() > 0 ? prelieviAllevamento.Sum(p => p.Quantita.Value) : (decimal?)null
                });
            }

            dto.Serie.Add(serie);

            return dto;
        }

        private WidgetGraficoDto MakeSpiderChart(List<AnalisiDto> analisi, int idAllevamento)
        {
            var dto = new WidgetGraficoDto();

            dto.ValoriAsseX = new List<string>() { "Grasso", "Proteine", "Carica batterica", "Cellule somatiche" };

            var analisiNotNull = analisi.Where(a => a.Grasso.HasValue && a.Proteine.HasValue && a.CaricaBatterica.HasValue && a.CelluleSomatiche.HasValue).ToList();

            var analisiAllevamento = analisiNotNull.Where(a => a.IdAllevamento == idAllevamento).ToList();
            var analisiAltri = analisiNotNull.Where(a => a.IdAllevamento != idAllevamento).ToList();

            var serieAllevamento = MakeSerie("Allevamento", analisiNotNull, analisiAllevamento);
            var serieAltri = MakeSerie("Altri", analisiNotNull, analisiAltri);

            dto.Serie.Add(serieAllevamento);
            dto.Serie.Add(serieAltri);

            return dto;
        }


        private SerieDto MakeSerie(string nome, List<AnalisiDto> analisiAll, List<AnalisiDto> analisi)
        {
            var serie = new SerieDto() { Nome = nome };

            if(analisi.Count > 0)
            {
                serie.Valori.Add(GetPercentile(analisiAll.Select(a => a.Grasso.Value).Distinct().ToArray(), analisi.Average(a => a.Grasso.Value)));
                serie.Valori.Add(GetPercentile(analisiAll.Select(a => a.Proteine.Value).Distinct().ToArray(), analisi.Average(a => a.Proteine.Value)));
                serie.Valori.Add(GetPercentile(analisiAll.Select(a => a.CaricaBatterica.Value).Distinct().ToArray(), analisi.Average(a => a.CaricaBatterica.Value)));
                serie.Valori.Add(GetPercentile(analisiAll.Select(a => a.CelluleSomatiche.Value).Distinct().ToArray(), analisi.Average(a => a.CelluleSomatiche.Value)));
            }

            return serie;
        }

        private List<AnalisiDto> GetAnalisi(DateTime from, DateTime to)
        {
            var entities = this.analisiRepository.DbSet
                .Where(a => from <= a.DataPrelievo)
                .Where(a => a.DataPrelievo < to)
                .ToList();

            return this.mapper.Map<List<AnalisiDto>>(entities);
        }

        public decimal GetPercentile(decimal[] sequence, decimal value)
        {
            var sortedSequence = sequence.OrderBy(d => d).ToArray();

            var min = sortedSequence.First();
            var max = sortedSequence.Last();

            if (value <= min)
                return 0;

            if (value >= max)
                return 100;

            //var index = sortedSequence
            //    .Select((d, i) => new { Value = d, Index = i })
            //    .First(v => v.Value >= value)
            //    .Index;
            var index = sortedSequence.TakeWhile(d => d < value).Count();
            

            return Math.Round((Convert.ToDecimal(index) / sortedSequence.Length) * 100, 2);

        }

        //public decimal GetPercentile(decimal[] sequence, decimal value)
        //{
        //    if (sequence.Length == 0)
        //        return 0;

        //    if (value > 100)
        //        value = 100;

        //    Array.Sort(sequence);
        //    int N = sequence.Length;
        //    decimal n = (N - 1) * (value / 100) + 1;
        //    // Another method: double n = (N + 1) * excelPercentile;
        //    if (n == 1) return sequence[0];
        //    else if (n == N) return sequence[N - 1];
        //    else
        //    {
        //        int k = (int)n;
        //        decimal d = n - k;
        //        return sequence[k - 1] + d * (sequence[k] - sequence[k - 1]);
        //    }
        //}

        //public decimal GetPercentile(decimal[] sequence, decimal excelPercentile)
        //{
        //    Array.Sort(sequence);
        //    int N = sequence.Length;
        //    decimal n = (N - 1) * excelPercentile + 1;
        //    // Another method: double n = (N + 1) * excelPercentile;
        //    if (n == 1) return sequence[0];
        //    else if (n == N) return sequence[N - 1];
        //    else
        //    {
        //        int k = (int)n;
        //        decimal d = n - k;
        //        return sequence[k - 1] + d * (sequence[k] - sequence[k - 1]);
        //    }
        //}

        //private decimal GetPercentile(decimal[] data, decimal p)
        //{
        //    var sortedData = data.OrderBy(d => d).ToArray();

        //    if (p >= 100) 
        //        return sortedData[sortedData.Length - 1];

        //    decimal position = Convert.ToDecimal((sortedData.Length + 1) * p) / 100;

        //    decimal leftNumber = 0;
        //    decimal rightNumber = 0;

        //    decimal n = p / 100 * (sortedData.Length - 1) + 1;

        //    if (position >= 1)
        //    {
        //        leftNumber = sortedData[(int)System.Math.Floor(n) - 1];
        //        rightNumber = sortedData[(int)System.Math.Floor(n)];
        //    }
        //    else
        //    {
        //        leftNumber = sortedData[0]; // first data
        //        rightNumber = sortedData[1]; // first data
        //    }

        //    if (leftNumber == rightNumber)
        //        return leftNumber;
        //    else
        //    {
        //        decimal part = n - System.Math.Floor(n);
        //        return leftNumber + part * (rightNumber - leftNumber);
        //    }
        //}

        #endregion

    }
}
