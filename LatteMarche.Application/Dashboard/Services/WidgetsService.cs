using LatteMarche.Application.Dashboard.Dtos;
using LatteMarche.Application.Dashboard.Interfaces;
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
        #region Fields

        private IRepository<PrelievoLatte, int> repository;
        private IRepository<Allevamento, int> allevamentiRepository;
        private IUtentiService utentiService;

        #endregion

        #region Constructor

        public WidgetsService(IUnitOfWork uow, IUtentiService utentiService)
        {
            this.repository = uow.Get<PrelievoLatte, int>();
            this.allevamentiRepository = uow.Get<Allevamento, int>();
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        public WidgetSommarioDto WidgetSommario(int idUtente)
        {
            var widgetDto = new WidgetSommarioDto();

            var query = GetQuery(idUtente);

            // settimana
            var inizioSettimana = GetInizioSettimana(DateTime.Today);
            var fineSettimana = inizioSettimana.AddDays(7);
            widgetDto.Qta_Settimanale = query
                .Where(p => inizioSettimana <= p.DataPrelievo && p.DataPrelievo <= fineSettimana)
                .Sum(p => p.Quantita);

            // mese
            var inizioMese = GetInizioMese(DateTime.Today);
            var fineMese = inizioMese.AddMonths(1);
            widgetDto.Qta_Mensile = query
                .Where(p => inizioMese <= p.DataPrelievo && p.DataPrelievo <= fineMese)
                .Sum(p => p.Quantita);

            // anno
            var inizioAnno = GetInizioAnno(DateTime.Today);
            var fineAnno = inizioAnno.AddYears(1);
            widgetDto.Qta_Mensile = query
                .Where(p => inizioAnno <= p.DataPrelievo && p.DataPrelievo <= fineAnno)
                .Sum(p => p.Quantita);

            return widgetDto;
        }



        public WidgetGraficoDto WidgetAcquirenti(int idUtente)
        {
            var widgetDto = new WidgetGraficoDto();

            var query = GetQuery(idUtente);

            return widgetDto;
        }

        public WidgetGraficoDto WidgetTipiLatte(int idUtente)
        {
            var widgetDto = new WidgetGraficoDto();

            var query = GetQuery(idUtente);

            return widgetDto;
        }


        private IQueryable<PrelievoLatte> GetQuery(int idUtente)
        {
            var query = this.repository.DbSet;

            var utente = this.utentiService.Details(idUtente);

            switch (utente.IdProfilo)
            {
                case 1:     // Admin
                    return query;

                case 7:     // Acquirente
                    return query.Where(a => a.Id == utente.IdAcquirente);

                case 3:
                    // allevamenti associati all'utente
                    var allevamentiIds = this.allevamentiRepository.DbSet
                        .Where(a => a.IdUtente == idUtente)
                        .Select(a => a.Id).ToList();

                    return query.Where(a => allevamentiIds.Contains(a.IdAllevamento.Value));

                case 4:     // Laboratorio
                    return query.Where(a => a.IdLabAnalisi == idUtente);

                case 5:     // Trasportatore
                    return query.Where(a => a.IdTrasportatore == idUtente);

                case 6:     // Destinatario
                    return query.Where(a => a.IdDestinatario == utente.IdDestinatario);

                case 8:     // Cessionario
                    return query.Where(a => a.IdCessionario ==  utente.IdCessionario);

                default:
                    return query;

            }

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
            var year = today.Month > 8 ? today.Year : today.Year - 1;

            return new DateTime(year, 8, 1);
        }

        #endregion

    }
}
