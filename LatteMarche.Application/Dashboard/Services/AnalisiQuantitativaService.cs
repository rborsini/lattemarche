using AutoMapper;
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
    public class AnalisiQuantitativaService : IAnalisiQuantitativaService
    {
        #region Constants

        private List<string> MONTH_NAMES = new List<string>() { "gen", "feb", "mar", "apr", "mag", "giu", "lug", "ago", "set", "ott", "nov", "dic" };

        #endregion

        #region Fields

        private IUnitOfWork uow;
        private IRepository<V_PrelievoLatte, int> prelieviRepository;

        #endregion

        #region Constructor

        public AnalisiQuantitativaService(IUnitOfWork uow)
        {
            this.uow = uow;
            this.prelieviRepository = this.uow.Get<V_PrelievoLatte, int>();
        }

        #endregion

        #region Methods

        public WidgetAnalisiQuantitativaDto Load(int idAllevamento, DateTime from, DateTime to)
        {
            var dto = new WidgetAnalisiQuantitativaDto();

            dto.Records = MakeRecords(idAllevamento, from, to);
            dto.AndamentoGiornaliero = MakeAndamentoGiornaliero(dto.Records, from, to);
            dto.AndamentoMensile = MakeAndamentoMensile(idAllevamento, from, to);

            return dto;
        }

        /// <summary>
        /// Caricamento record per tabelle e per grafico andamento giornaliero
        /// </summary>
        /// <param name="idAllevamento"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private List<WidgetAnalisiQuantitativaDto.Record> MakeRecords(int idAllevamento, DateTime from, DateTime to)
        {
            var prelievi = LoadPrelievi(idAllevamento, from, to).ToList();
            return Mapper.Map<List<WidgetAnalisiQuantitativaDto.Record>>(prelievi);
        }

        /// <summary>
        /// Generazione modello grafico andamento giornaliero
        /// </summary>
        /// <param name="records"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private WidgetGraficoDto MakeAndamentoGiornaliero(List<WidgetAnalisiQuantitativaDto.Record> records, DateTime from, DateTime to)
        {
            var valoriAsseX = new List<string>();
            var serie = new SerieDto() { Nome = "kg" };

            var date = from;
            while(date < to)
            {
                valoriAsseX.Add($"{date:dd}");

                var prelieviGiorno = records.Where(r => date <= r.Data && r.Data < date.AddDays(1));
                var valore = prelieviGiorno.Count() == 0 ? 0 : prelieviGiorno.Sum(p => p.Qta_Kg);

                serie.Valori.Add(valore);

                date = date.AddDays(1);
            }

            return new WidgetGraficoDto()
            {
                ValoriAsseX = valoriAsseX,
                Serie = new List<SerieDto>() { serie }
            };
        }

        /// <summary>
        /// Generazione modello grafico andamento mensile
        /// </summary>
        /// <param name="idAllevamento"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private WidgetGraficoDto MakeAndamentoMensile(int idAllevamento, DateTime from, DateTime to)
        {
            var valoriAsseX = new List<string>();
            var serie = new SerieDto() { Nome = "kg" };

            var end = NextMonthStart(to);
            var start = end.AddYears(-1);

            while (start < end)
            {
                valoriAsseX.Add(MONTH_NAMES[start.Month - 1]);

                var prelieviGiorno = LoadPrelievi(idAllevamento, start, start.AddMonths(1));
                var valore = prelieviGiorno.Count() == 0 ? 0 : prelieviGiorno.Sum(p => p.Quantita);

                serie.Valori.Add(valore);

                start = start.AddMonths(1);
            }

            return new WidgetGraficoDto()
            {
                ValoriAsseX = valoriAsseX,
                Serie = new List<SerieDto>() { serie }
            };
        }

        /// <summary>
        /// Caricamento prelievi periodo 
        /// </summary>
        /// <param name="idAllevamento"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private IQueryable<V_PrelievoLatte> LoadPrelievi(int idAllevamento, DateTime from, DateTime to)
        {
            return this.prelieviRepository.DbSet.Where(p => p.IdAllevamento == idAllevamento && from <= p.DataPrelievo && p.DataPrelievo < to);
        }

        /// <summary>
        /// Ritorna l'inizio del prossimo mese
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private DateTime NextMonthStart(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1);
        }

        //public WidgetAnalisiQuantitativaDto Load(int idAllevamento, DateTime? from, DateTime to)
        //{
        //    var dto = new WidgetAnalisiQuantitativaDto();

        //    dto.AndamentoGiornaliero.ValoriAsseX = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        //    dto.AndamentoGiornaliero.Serie.Add(new SerieDto()
        //    {
        //        Id = "latte",
        //        Nome = "kg",
        //        Valori = new List<decimal?>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
        //    });

        //    dto.AndamentoMensile.ValoriAsseX = new List<string>() { "gen", "feb", "mar", "apr", "mag", "giu", "lug", "ago", "set", "ott", "nov", "dic" };
        //    dto.AndamentoMensile.Serie.Add(new SerieDto()
        //    {
        //        Id = "latte",
        //        Nome = "kg",
        //        Valori = new List<decimal?>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }
        //    });

        //    dto.Records = new List<WidgetAnalisiQuantitativaDto.Record>()
        //    {
        //        new WidgetAnalisiQuantitativaDto.Record()
        //        {
        //            Acquirente = "LatteMarche",
        //            Data = DateTime.Today,
        //            Destinatario = "Cooperlat",
        //            Qta_Kg = 10,
        //            Qta_Lt = 12,
        //            Temperatura = 3,
        //            TipoLatte = "Crudo",
        //            Trasportatore = "Fattorie Marchigiane"
        //        }
        //    };

        //    return dto;
        //}

        #endregion

    }
}
