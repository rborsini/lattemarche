using AutoMapper;
using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Application.Destinatari.Dtos;
using LatteMarche.Application.Destinatari.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Application;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Destinatari.Services
{
    public class DestinatariService : EntityService<Destinatario, int, DestinatarioDto>, IDestinatariService
    {

        #region Fields

        private IRepository<Allevamento, int> allevamentiRepository;
        private IRepository<PrelievoLatte, int> prelieviRepository;

        private IUtentiService utentiService;

        #endregion

        #region Constructors

        public DestinatariService(IUnitOfWork uow, IMapper mapper, IUtentiService utentiService)
            : base(uow, mapper)
        {
            this.allevamentiRepository = this.uow.Get<Allevamento, int>();
            this.prelieviRepository = this.uow.Get<PrelievoLatte, int>();
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        public override List<DestinatarioDto> Index()
        {
            var entities = this.repository.DbSet.Where(d => d.Abilitato).ToList();

            return this.mapper.Map<List<DestinatarioDto>>(entities);
        }

        public DropDownDto DropDown(int? idUtente = null)
        {
            var dropdown = new DropDownDto();

            var query = GetQuery(idUtente);

            dropdown.Items = query
                .Where(d => d.Abilitato)
                .ToList()
                .Select(c => new DropDownItem()
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.RagioneSociale} - {c.Stabilimento}"
                })
                .OrderBy(i => i.Text)
                .ToList();

            return dropdown;
        }


        private IQueryable<Destinatario> GetQuery(int? idUtente = null)
        {
            var query = this.repository.DbSet
                .Where(d => d.Abilitato);

            if (!idUtente.HasValue)
                return query;

            var utente = this.utentiService.Details(idUtente.Value);
            var destinatariIds = new List<int?>();

            switch (utente.IdProfilo)
            {
                case 1:     // Admin
                    return query;

                case 7:     // Acquirente
                    destinatariIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdAcquirente == utente.IdAcquirente)
                        .Select(p => p.IdDestinatario)
                        .Distinct()
                        .ToList();
                    return query.Where(a => destinatariIds.Contains(a.Id));

                case 3:
                    // allevamenti associati all'utente
                    var allevamentiIds = this.allevamentiRepository.DbSet
                        .Where(a => a.IdUtente == idUtente.Value)
                        .Select(a => a.Id).ToList();

                    // acquirenti associati a prelievi effettuati presso gli allevamenti relazionati
                    destinatariIds = this.prelieviRepository.DbSet
                        .Where(p => allevamentiIds.Contains(p.IdAllevamento.Value))
                        .Select(p => p.IdDestinatario)
                        .Distinct()
                        .ToList();
                    return query.Where(a => destinatariIds.Contains(a.Id));

                case 4:     // Laboratorio
                    destinatariIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdLabAnalisi == idUtente.Value)
                        .Select(p => p.IdDestinatario)
                        .Distinct()
                        .ToList();
                    return query.Where(a => destinatariIds.Contains(a.Id));

                case 5:     // Trasportatore
                    destinatariIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdTrasportatore == idUtente.Value)
                        .Select(p => p.IdDestinatario)
                        .Distinct()
                        .ToList();
                    return query.Where(a => destinatariIds.Contains(a.Id));

                case 6:     // Destinatario
                    return query.Where(a => a.Id == utente.IdDestinatario);

                case 8:     // Cessionario
                    destinatariIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdCessionario == utente.IdCessionario)
                        .Select(p => p.IdDestinatario)
                        .Distinct()
                        .ToList();
                    return query.Where(a => destinatariIds.Contains(a.Id));

                default:
                    return query;

            }


        }

        protected override Destinatario UpdateProperties(Destinatario viewEntity, Destinatario dbEntity)
        {
            dbEntity.IdComune = viewEntity.IdComune;
            dbEntity.Indirizzo = viewEntity.Indirizzo;
            dbEntity.P_IVA = viewEntity.P_IVA;
            dbEntity.RagioneSociale = viewEntity.RagioneSociale;
            dbEntity.Stabilimento = viewEntity.Stabilimento;

            return dbEntity;
        }

        #endregion
    }
}
