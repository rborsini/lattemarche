using LatteMarche.Application.Dashboard.Dtos;
using LatteMarche.Application.Dashboard.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dashboard.Services
{
    public class AnalisiQualitativaService : IAnalisiQualitativaService
    {
        WidgetAnalisiQualitativaDto IAnalisiQualitativaService.Load(int idAllevamento, DateTime from, DateTime to)
        {
            var dto = new WidgetAnalisiQualitativaDto();

            dto.Grasso_Proteine.ValoriAsseX = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            dto.Grasso_Proteine.Serie.Add(new SerieDto()
            {
                Id = "grasso",
                Nome = "Grasso",
                Valori = new List<decimal?>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
            });
            dto.Grasso_Proteine.Serie.Add(new SerieDto()
            {
                Id = "proteine",
                Nome = "Proteine",
                Valori = new List<decimal?>() { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 }
            });

            dto.CaricaBatterica_CelluleSomatiche.ValoriAsseX = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            dto.CaricaBatterica_CelluleSomatiche.Serie.Add(new SerieDto()
            {
                Id = "carica_batterica",
                Nome = "Carica batterica",
                Valori = new List<decimal?>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
            });
            dto.CaricaBatterica_CelluleSomatiche.Serie.Add(new SerieDto()
            {
                Id = "cellule_somatiche",
                Nome = "Cellule somatiche",
                Valori = new List<decimal?>() { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 }
            });


            dto.Records = new List<WidgetAnalisiQualitativaDto.Record>()
            {
                new WidgetAnalisiQualitativaDto.Record()
                {
                    Campione = "1234",
                    CaricaBatterica = 1,
                    CelluleSomatiche = 2,
                    CodiceASL = "ABCD",
                    DataAccettazione = DateTime.Today,
                    DataPrelievo = DateTime.Today.AddDays(-1),
                    DataRapporto = DateTime.Today.AddDays(1),
                    Grasso = 3,
                    Proteine = 4
                }
            };

            return dto;
        }
    }
}
