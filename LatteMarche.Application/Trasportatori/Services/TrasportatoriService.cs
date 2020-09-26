using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.Trasportatori.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Trasportatori.Services
{
    public class TrasportatoriService : ITrasportatoriService
    {

        #region Fields

        private IUnitOfWork uow;

        private IRepository<Utente, int> utentiRepository;
        private IRepository<Allevamento, int> allevamentiRepository;
        private IRepository<PrelievoLatte, int> prelieviRepository;

        private IUtentiService utentiService;

        #endregion

        #region Constructors

        public TrasportatoriService(IUnitOfWork uow, IUtentiService utentiService)
        {
            this.uow = uow;

            this.utentiRepository = this.uow.Get<Utente, int>();
            this.allevamentiRepository = this.uow.Get<Allevamento, int>();
            this.prelieviRepository = this.uow.Get<PrelievoLatte, int>();

            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        public DropDownDto DropDown(int? idUtente = null)
        {
            var dropdown = new DropDownDto();

            var query = GetQuery(idUtente);

            dropdown.Items = query
                .Where(u => u.Abilitato)
                .ToList()
                .Select(c => new DropDownItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.RagioneSociale
                })
                .OrderBy(i => i.Text)
                .ToList();

            return dropdown;
        }

        private IQueryable<Utente> GetQuery(int? idUtente = null)
        {
            var query = this.utentiRepository.DbSet
                .Where(u => u.Abilitato)
                .Where(u => u.IdProfilo == 5);

            if (!idUtente.HasValue)
                return query;

            var utente = this.utentiService.Details(idUtente.Value);
            var trasportatoriIds = new List<int?>();

            switch (utente.IdProfilo)
            {
                case 1:     // Admin
                    return query;

                case 7:     // Acquirente
                    // trasportatori associati a prelievi effettuati presso gli acquirenti relazionati
                    trasportatoriIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdAcquirente == utente.IdAcquirente)
                        .Select(p => p.IdTrasportatore)
                        .Distinct()
                        .ToList();

                    return query.Where(a => trasportatoriIds.Contains(a.Id));

                case 3:     // Allevatore
                    // allevamenti associati all'utente
                    var allevamentiIds = this.allevamentiRepository.DbSet
                        .Where(a => a.IdUtente == idUtente.Value)
                        .Select(a => a.Id).ToList();

                    // trasportatori associati a prelievi effettuati presso gli allevamenti relazionati
                    trasportatoriIds = this.prelieviRepository.DbSet
                        .Where(p => allevamentiIds.Contains(p.IdAllevamento.Value))
                        .Select(p => p.IdTrasportatore)
                        .Distinct()
                        .ToList();
                    return query.Where(a => trasportatoriIds.Contains(a.Id));

                case 4:     // Laboratorio
                    trasportatoriIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdLabAnalisi == idUtente.Value)
                        .Select(p => p.IdTrasportatore)
                        .Distinct()
                        .ToList();
                    return query.Where(a => trasportatoriIds.Contains(a.Id));

                case 5:     // Trasportatore
                    return query.Where(u => u.Id == utente.Id);

                case 6:     // Destinatario
                    trasportatoriIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdDestinatario == utente.IdDestinatario)
                        .Select(p => p.IdTrasportatore)
                        .Distinct()
                        .ToList();
                    return query.Where(a => trasportatoriIds.Contains(a.Id));

                case 8:     // Cessionario
                    trasportatoriIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdCessionario == utente.IdCessionario)
                        .Select(p => p.IdTrasportatore)
                        .Distinct()
                        .ToList();
                    return query.Where(a => trasportatoriIds.Contains(a.Id));

                default:
                    return query;

            }

        }

        #endregion
    }
}
