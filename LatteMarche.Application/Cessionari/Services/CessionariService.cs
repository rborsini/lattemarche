using LatteMarche.Application.Cessionari.Dtos;
using LatteMarche.Application.Cessionari.Interfaces;
using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Application;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Cessionari.Services
{
    public class CessionariService : EntityService<Cessionario, int, CessionarioDto>, ICessionariService
    {
        #region Fields

        private IRepository<Allevamento, int> allevamentiRepository;
        private IRepository<PrelievoLatte, int> prelieviRepository;

        private IUtentiService utentiService;

        #endregion

        #region Constructors

        public CessionariService(IUnitOfWork uow, IUtentiService utentiService)
            : base(uow)
        {
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
                .Select(c => new DropDownItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.RagioneSociale
                })
                .OrderBy(i => i.Text)
                .ToList();

            return dropdown;
        }

        private IQueryable<Cessionario> GetQuery(int? idUtente = null)
        {
            var query = this.repository.DbSet;

            if (!idUtente.HasValue)
                return query;

            var utente = this.utentiService.Details(idUtente.Value);
            var cessionariIds = new List<int?>();

            switch (utente.IdProfilo)
            {
                case 1:     // Admin
                    return query;

                case 7:     // Acquirente
                    cessionariIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdAcquirente == utente.IdAcquirente)
                        .Select(p => p.IdCessionario)
                        .Distinct()
                        .ToList();
                    return query.Where(a => cessionariIds.Contains(a.Id));

                case 3:
                    // allevamenti associati all'utente
                    var allevamentiIds = this.allevamentiRepository.DbSet
                        .Where(a => a.IdUtente == idUtente.Value)
                        .Select(a => a.Id).ToList();

                    // acquirenti associati a prelievi effettuati presso gli allevamenti relazionati
                    cessionariIds = this.prelieviRepository.DbSet
                        .Where(p => allevamentiIds.Contains(p.IdAllevamento.Value))
                        .Select(p => p.IdCessionario)
                        .Distinct()
                        .ToList();
                    return query.Where(a => cessionariIds.Contains(a.Id));

                case 4:     // Laboratorio
                    cessionariIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdLabAnalisi == idUtente.Value)
                        .Select(p => p.IdCessionario)
                        .Distinct()
                        .ToList();
                    return query.Where(a => cessionariIds.Contains(a.Id));

                case 5:     // Trasportatore
                    cessionariIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdTrasportatore == idUtente.Value)
                        .Select(p => p.IdCessionario)
                        .Distinct()
                        .ToList();
                    return query.Where(a => cessionariIds.Contains(a.Id));

                case 6:     // Destinatario
                    cessionariIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdDestinatario == utente.IdDestinatario)
                        .Select(p => p.IdCessionario)
                        .Distinct()
                        .ToList();
                    return query.Where(a => cessionariIds.Contains(a.Id));

                case 8:     // Cessionario
                    return query.Where(a => a.Id == utente.IdCessionario);

                default:
                    return query;

            }


        }

        protected override Cessionario UpdateProperties(Cessionario viewEntity, Cessionario dbEntity)
        {
            dbEntity.IdComune = viewEntity.IdComune;
            dbEntity.Indirizzo = viewEntity.Indirizzo;
            dbEntity.Piva = viewEntity.Piva;
            dbEntity.RagioneSociale = viewEntity.RagioneSociale;

            return dbEntity;
        }

        #endregion
    }
}
