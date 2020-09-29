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

        private IUnitOfWork uow;
        private IRepository<PrelievoLatte, int> prelieviRepository;

        #endregion

        #region Constructor

        public AnalisiComparativaService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        #endregion

        #region Methods

        public WidgetAnalisiComparativaDto Load(int idAllevamento, DateTime from, DateTime to)
        {
            var dto = new WidgetAnalisiComparativaDto();

            dto.BubbleChart.Serie.Add(new SerieDto()
            {
                Bolle = new List<BollaDto>()
                {
                    new BollaDto() { Nome = "Giulio",   X = 95,     Y = 95,     Z = 75 },
                    new BollaDto() { Nome = "altri",    X = 100,    Y = 80,     Z = 75 }
                }
            });


            dto.SpiderChart.ValoriAsseX = new List<string>() { "Grasso", "Proteine", "Carica batterica", "Cellule somatiche" };
            dto.SpiderChart.Serie.Add(new SerieDto()
            {
                Nome = "Viola Giulio",
                Valori = new List<decimal?>() { 1, 2, 3, 4 }
            });
            dto.SpiderChart.Serie.Add(new SerieDto()
            {
                Nome = "Altri",
                Valori = new List<decimal?>() { 4, 3, 2, 1 }
            });

            return dto;
        }

        #endregion

    }
}
