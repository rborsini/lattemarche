using LatteMarche.Application.Acquirenti.Interfaces;
using LatteMarche.Application.Dashboard.Dtos;
using LatteMarche.Application.Dashboard.Interfaces;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Dashboard.Services
{
    public class WidgetsService : IWidgetsService
    {
        #region Constants

        public const int MESE_INIZIO_STAGIONE = 7;

        #endregion

        #region Fields

        private IPrelieviLatteService prelieviService;
        private IAcquirentiService acquirentiService;
        private ITipiLatteService tipiLatteService;

        #endregion

        #region Constructor

        public WidgetsService(IUnitOfWork uow, IPrelieviLatteService prelieviService, IAcquirentiService acquirentiService, ITipiLatteService tipiLatteService)
        {
            this.prelieviService = prelieviService;
            this.acquirentiService = acquirentiService;
            this.tipiLatteService = tipiLatteService;
        }

        #endregion

        #region Methods

        public WidgetSommarioDto WidgetSommario(int idUtente)
        {
            var widgetDto = new WidgetSommarioDto();

            //var dataRiferimento = new DateTime(2018, 1, 1);
            var dataRiferimento = DateTime.Today;
            var query = this.prelieviService.PrelieviAutorizzati(idUtente);

            // settimana
            var inizioSettimana = GetInizioSettimana(dataRiferimento);
            var fineSettimana = inizioSettimana.AddDays(7);
            widgetDto.Qta_Settimanale = query
                .Where(p => inizioSettimana <= p.DataPrelievo && p.DataPrelievo <= fineSettimana)
                .Sum(p => p.Quantita);

            // mese
            var inizioMese = GetInizioMese(dataRiferimento);
            var fineMese = inizioMese.AddMonths(1);
            widgetDto.Qta_Mensile = query
                .Where(p => inizioMese <= p.DataPrelievo && p.DataPrelievo <= fineMese)
                .Sum(p => p.Quantita);

            // anno
            var inizioAnno = GetInizioAnno(dataRiferimento);
            var fineAnno = inizioAnno.AddYears(1);
            widgetDto.Qta_Annuale = query
                .Where(p => inizioAnno <= p.DataPrelievo && p.DataPrelievo <= fineAnno)
                .Sum(p => p.Quantita);

            return widgetDto;
        }

        public WidgetGraficoDto WidgetAcquirenti(int idUtente)
        {
            var widgetDto = new WidgetGraficoDto();

            var acquirentiAutorizzati = this.acquirentiService.DropDown(idUtente);

            foreach(var item in acquirentiAutorizzati.Items)
            {
                widgetDto.Serie.Add(new SerieDto()
                {
                    Id = item.Value,
                    Nome = item.Text
                });
            }


            var query = this.prelieviService.PrelieviAutorizzati(idUtente);

            //var dataRiferimento = new DateTime(2018, 1, 1);
            var dataRiferimento = DateTime.Today;
            var meseCorrente = GetInizioAnno(dataRiferimento);
            var fineAnno = meseCorrente.AddYears(1).AddMonths(1);

            while(meseCorrente < fineAnno)
            {

                widgetDto.ValoriAsseX.Add($"{meseCorrente.Year}-{meseCorrente.Month}");

                var meseSuccessivo = meseCorrente.AddMonths(1);

                var queryMensile = query.Where(p => meseCorrente <= p.DataPrelievo && p.DataPrelievo < meseSuccessivo);

                var result = queryMensile
                    .Where(p => p.IdAcquirente.HasValue)
                    .GroupBy(p => p.IdAcquirente, (k, c) => new
                    {
                        IdAcquirente = k.Value,
                        Quantita = c.Sum(p => p.Quantita)
                    })
                    .ToList();

                foreach(var serie in widgetDto.Serie)
                {
                    var valore = result.FirstOrDefault(r => $"{r.IdAcquirente}" == serie.Id);
                    serie.Valori.Add(valore == null ? (decimal?)null : valore.Quantita);                    
                }

                meseCorrente = meseSuccessivo;
            }

            return widgetDto;
        }

        public WidgetGraficoDto WidgetTipiLatte(int idUtente)
        {
            var widgetDto = new WidgetGraficoDto();

            var tipiLatte = this.tipiLatteService.DropDown();

            foreach (var item in tipiLatte.Items)
            {
                widgetDto.Serie.Add(new SerieDto()
                {
                    Id = item.Value,
                    Nome = item.Text
                });
            }

            var query = this.prelieviService.PrelieviAutorizzati(idUtente);

            var dataRiferimento = DateTime.Today;
            var meseCorrente = GetInizioAnno(dataRiferimento);
            var fineAnno = meseCorrente.AddYears(1).AddMonths(1);

            while (meseCorrente < fineAnno)
            {

                widgetDto.ValoriAsseX.Add($"{meseCorrente.Year}-{meseCorrente.Month}");

                var meseSuccessivo = meseCorrente.AddMonths(1);

                var queryMensile = query.Where(p => meseCorrente <= p.DataPrelievo && p.DataPrelievo < meseSuccessivo);

                var result = queryMensile
                    .Where(p => p.IdTipoLatte.HasValue)
                    .GroupBy(p => p.IdTipoLatte, (k, c) => new
                    {
                        IdTipoLatte = k.Value,
                        Quantita = c.Sum(p => p.Quantita)
                    })
                    .ToList();

                foreach (var serie in widgetDto.Serie)
                {
                    var valore = result.FirstOrDefault(r => $"{r.IdTipoLatte}" == serie.Id);
                    serie.Valori.Add(valore == null ? (decimal?)null : valore.Quantita);
                }

                meseCorrente = meseSuccessivo;
            }

            return widgetDto;
        }

        private DateTime GetInizioSettimana(DateTime data)
        {
            int diff = (7 + (data.DayOfWeek - DayOfWeek.Monday)) % 7;
            return data.AddDays(-1 * diff).Date;
        }

        private DateTime GetInizioMese(DateTime today)
        {
            return new DateTime(today.Year, today.Month, 1);
        }

        /// <summary>
        /// La stagione del latte inizia ad agosto
        /// </summary>
        /// <param name="today"></param>
        /// <returns></returns>
        private DateTime GetInizioAnno(DateTime today)
        {
            var year = today.Month > MESE_INIZIO_STAGIONE ? today.Year : today.Year - 1;

            return new DateTime(year, MESE_INIZIO_STAGIONE, 1);
        }

        #endregion

    }
}
