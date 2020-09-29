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
            dto.Grasso_Proteine = MakeGrassoProteine(dto.Records, from, to);
            dto.CaricaBatterica_CelluleSomatiche = MakeCaricaBattericaCelluleSomatiche(dto.Records, from, to);

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

        private WidgetGraficoDto MakeGrassoProteine(List<WidgetAnalisiQualitativaDto.Record> records, DateTime from, DateTime to)
        {
            var valoriAsseX = new List<string>();
            var serieGrasso = new SerieDto();
            var serieProteine = new SerieDto();

            var date = from;
            while (date < to)
            {
                valoriAsseX.Add($"{date:dd}");

                var analisiGiorno = records.Where(r => date <= r.DataPrelievo && r.DataPrelievo < date.AddDays(1));
                if(analisiGiorno.Count() == 1)
                {
                    var analisi = analisiGiorno.First();
                    serieGrasso.Valori.Add(analisi.Grasso);
                    serieProteine.Valori.Add(analisi.Proteine);
                }
                else
                {
                    serieGrasso.Valori.Add((decimal?)null);
                    serieProteine.Valori.Add((decimal?)null);
                }               

                date = date.AddDays(1);
            }

            return new WidgetGraficoDto()
            {
                ValoriAsseX = valoriAsseX,
                Serie = new List<SerieDto>() { serieGrasso, serieProteine }
            };
        }

        private WidgetGraficoDto MakeCaricaBattericaCelluleSomatiche(List<WidgetAnalisiQualitativaDto.Record> records, DateTime from, DateTime to)
        {
            var valoriAsseX = new List<string>();
            var serieCaricaBatterica = new SerieDto();
            var serieCelluleSomatiche = new SerieDto();

            var date = from;
            while (date < to)
            {
                valoriAsseX.Add($"{date:dd}");

                var analisiGiorno = records.Where(r => date <= r.DataPrelievo && r.DataPrelievo < date.AddDays(1));
                if (analisiGiorno.Count() == 1)
                {
                    var analisi = analisiGiorno.First();
                    serieCaricaBatterica.Valori.Add(analisi.CaricaBatterica);
                    serieCelluleSomatiche.Valori.Add(analisi.CelluleSomatiche);
                }
                else
                {
                    serieCaricaBatterica.Valori.Add((decimal?)null);
                    serieCelluleSomatiche.Valori.Add((decimal?)null);
                }

                date = date.AddDays(1);
            }

            return new WidgetGraficoDto()
            {
                ValoriAsseX = valoriAsseX,
                Serie = new List<SerieDto>() { serieCaricaBatterica, serieCelluleSomatiche }
            };
        }

        //public WidgetAnalisiQualitativaDto IAnalisiQualitativaService.Load(int idAllevamento, DateTime from, DateTime to)
        //{
        //    var dto = new WidgetAnalisiQualitativaDto();

        //    dto.Grasso_Proteine.ValoriAsseX = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        //    dto.Grasso_Proteine.Serie.Add(new SerieDto()
        //    {
        //        Id = "grasso",
        //        Nome = "Grasso",
        //        Valori = new List<decimal?>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
        //    });
        //    dto.Grasso_Proteine.Serie.Add(new SerieDto()
        //    {
        //        Id = "proteine",
        //        Nome = "Proteine",
        //        Valori = new List<decimal?>() { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 }
        //    });

        //    dto.CaricaBatterica_CelluleSomatiche.ValoriAsseX = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        //    dto.CaricaBatterica_CelluleSomatiche.Serie.Add(new SerieDto()
        //    {
        //        Id = "carica_batterica",
        //        Nome = "Carica batterica",
        //        Valori = new List<decimal?>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
        //    });
        //    dto.CaricaBatterica_CelluleSomatiche.Serie.Add(new SerieDto()
        //    {
        //        Id = "cellule_somatiche",
        //        Nome = "Cellule somatiche",
        //        Valori = new List<decimal?>() { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 }
        //    });


        //    dto.Records = new List<WidgetAnalisiQualitativaDto.Record>()
        //    {
        //        new WidgetAnalisiQualitativaDto.Record()
        //        {
        //            Campione = "1234",
        //            CaricaBatterica = 1,
        //            CelluleSomatiche = 2,
        //            CodiceASL = "ABCD",
        //            DataAccettazione = DateTime.Today,
        //            DataPrelievo = DateTime.Today.AddDays(-1),
        //            DataRapporto = DateTime.Today.AddDays(1),
        //            Grasso = 3,
        //            Proteine = 4
        //        }
        //    };

        //    return dto;
        //}

        #endregion

    }
}
