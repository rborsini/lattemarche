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

        private IRepository<Analisi, string> analisiRepository;
        private IRepository<PrelievoLatte, int> prelieviRepository;

        #endregion

        #region Constructor

        public AnalisiComparativaService(IUnitOfWork uow)
        {
            this.analisiRepository = uow.Get<Analisi, string>();
            this.prelieviRepository = uow.Get<PrelievoLatte, int>();
        }

        #endregion

        #region Methods

        public WidgetAnalisiComparativaDto Load(int idAllevamento, DateTime from, DateTime to)
        {
            var dto = new WidgetAnalisiComparativaDto();

            var analisi = GetAnalisi(from, to);

            dto.BubbleChart = MakeBubbleChart(analisi, from, to);
            dto.SpiderChart = MakeSpiderChart(analisi, idAllevamento);

            return dto;
        }



        private WidgetGraficoDto MakeBubbleChart(List<AnalisiDto> analisi, DateTime from, DateTime to)
        {
            var dto = new WidgetGraficoDto();
            var serie = new SerieDto();

            var idAllevamenti = analisi.Select(a => a.IdAllevamento).Distinct().ToList();

            foreach(var idAllevamento in idAllevamenti)
            {
                var analisiAllevamento = analisi.Where(a => a.IdAllevamento == idAllevamento).ToList();
                var prelieviAllevamento = this.prelieviRepository.DbSet.Where(p => p.IdAllevamento == idAllevamento && from <= p.DataPrelievo && p.DataPrelievo < to);
                var valoriGrasso = analisiAllevamento.Where(a => a.Grasso.HasValue).Select(a => a.Grasso);
                var valoriProteine = analisiAllevamento.Where(a => a.Proteine.HasValue).Select(a => a.Proteine);

                serie.Bolle.Add(new BollaDto()
                {
                    Nome = analisiAllevamento[0].NomeProduttore,
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
            //var serieAllevamento = new SerieDto() { Nome = "Allevamento" };
            //var serieAltri = new SerieDto() { Nome = "Altri" };

            var analisiAllevamento = analisi.Where(a => a.IdAllevamento == idAllevamento).ToList();
            var analisiAltri = analisi.Where(a => a.IdAllevamento != idAllevamento).ToList();

            var serieAllevamento = MakeSerie("Allevamento", analisiAllevamento);
            var serieAltri = MakeSerie("Altri", analisiAltri);

            //serieAllevamento.Valori.Add(analisiAllevamento.Sum(a => a.Grasso));
            //serieAllevamento.Valori.Add(analisiAllevamento.Sum(a => a.Proteine));
            //serieAllevamento.Valori.Add(analisiAllevamento.Sum(a => a.CaricaBatterica));
            //serieAllevamento.Valori.Add(analisiAllevamento.Sum(a => a.CelluleSomatiche));


            //serieAltri.Valori.Add(analisiAltri.Sum(a => a.Grasso));
            //serieAltri.Valori.Add(analisiAltri.Sum(a => a.Proteine));
            //serieAltri.Valori.Add(analisiAltri.Sum(a => a.CaricaBatterica));
            //serieAltri.Valori.Add(analisiAltri.Sum(a => a.CelluleSomatiche));

            dto.Serie.Add(serieAllevamento);
            dto.Serie.Add(serieAltri);

            return dto;
        }

        private SerieDto MakeSerie(string nome, List<AnalisiDto> analisi)
        {
            var serie = new SerieDto() { Nome = nome };

            var valoriGrasso = analisi.Where(a => a.Grasso.HasValue).Select(a => a.Grasso.Value);
            var valoriProteine = analisi.Where(a => a.Proteine.HasValue).Select(a => a.Proteine.Value);
            var valoriCaricaBatterica = analisi.Where(a => a.CaricaBatterica.HasValue).Select(a => a.CaricaBatterica.Value);
            var valoriCelluleSomatiche = analisi.Where(a => a.CelluleSomatiche.HasValue).Select(a => a.CelluleSomatiche.Value);

            serie.Valori.Add(valoriGrasso.Count() > 0 ? valoriGrasso.Average() : (decimal?)null);
            serie.Valori.Add(valoriProteine.Count() > 0 ? valoriProteine.Average() : (decimal?)null);
            serie.Valori.Add(valoriCaricaBatterica.Count() > 0 ? valoriCaricaBatterica.Average() : (decimal?)null);
            serie.Valori.Add(valoriCelluleSomatiche.Count() > 0 ? valoriCelluleSomatiche.Average() : (decimal?)null);

            return serie;
        }

        private List<AnalisiDto> GetAnalisi(DateTime from, DateTime to)
        {
            var entities = this.analisiRepository.DbSet
                .Where(a => from <= a.DataPrelievo)
                .Where(a => a.DataPrelievo < to)
                .ToList();

            return Mapper.Map<List<AnalisiDto>>(entities);
        }

        #endregion

    }
}
