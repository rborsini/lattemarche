using AutoMapper;
using LatteMarche.Application.AnalisiLatte.Dtos;
using LatteMarche.Application.AnalisiLatte.Interfaces;
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
    public class AnalisiQualitativaService : IAnalisiQualitativaService
    {

        #region Fields

        private IRepository<Analisi, string> analisiRepository;

        #endregion

        #region Constructor

        public AnalisiQualitativaService(IUnitOfWork uow)
        {
            this.analisiRepository = uow.Get<Analisi, string>();
        }

        #endregion

        #region Methods

        public WidgetAnalisiQualitativaDto Load(int idAllevamento, DateTime from, DateTime to)
        {
            var dto = new WidgetAnalisiQualitativaDto();

            dto.Records = MakeRecords(idAllevamento, from, to);
            dto.Grasso_Proteine = MakeGrassoProteine(dto.Records);
            dto.CaricaBatterica_CelluleSomatiche = MakeCaricaBattericaCelluleSomatiche(dto.Records);

            return dto;
        }

        private List<WidgetAnalisiQualitativaDto.Record> MakeRecords(int idAllevamento, DateTime from, DateTime to)
        {
            var analisi = this.analisiRepository.DbSet
                .Where(a => a.IdAllevamento == idAllevamento)
                .Where(a => from <= a.DataPrelievo)
                .Where(a => a.DataPrelievo < to)
                .ToList();

            var analisiDto = Mapper.Map<List<AnalisiDto>>(analisi);

            return Mapper.Map<List<WidgetAnalisiQualitativaDto.Record>>(analisiDto);

        }

        private WidgetGraficoDto MakeGrassoProteine(List<WidgetAnalisiQualitativaDto.Record> records)
        {
            var valoriAsseX = new List<string>();
            var serieGrasso = new SerieDto() { Nome = "Grasso" };
            var serieProteine = new SerieDto() { Nome = "Proteine", Y_Axis = 1 };
            
            foreach(var record in records)
            {
                valoriAsseX.Add($"{record.DataPrelievo:dd/MM}");
                serieGrasso.Valori.Add(record.Grasso);
                serieProteine.Valori.Add(record.Proteine);
            }

            return new WidgetGraficoDto()
            {
                ValoriAsseX = valoriAsseX,
                Serie = new List<SerieDto>() { serieGrasso, serieProteine }
            };
        }

        private WidgetGraficoDto MakeCaricaBattericaCelluleSomatiche(List<WidgetAnalisiQualitativaDto.Record> records)
        {
            var valoriAsseX = new List<string>();
            var serieCaricaBatterica = new SerieDto() { Nome = "Carica batterica" };
            var serieCelluleSomatiche = new SerieDto() { Nome = "Cellule somatiche", Y_Axis = 1 };

            foreach (var record in records)
            {
                valoriAsseX.Add($"{record.DataPrelievo:dd/MM}");
                serieCaricaBatterica.Valori.Add(record.CaricaBatterica);
                serieCelluleSomatiche.Valori.Add(record.CelluleSomatiche);
            }

            return new WidgetGraficoDto()
            {
                ValoriAsseX = valoriAsseX,
                Serie = new List<SerieDto>() { serieCaricaBatterica, serieCelluleSomatiche }
            };
        }

        #endregion

    }
}
