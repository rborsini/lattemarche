using LatteMarche.Application.AziendeTrasportatori.Dtos;
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

namespace LatteMarche.Application.AziendeTrasportatori.Interfaces
{
    public class AziendeTrasportatoriService : EntityService<AziendaTrasportatori, int, AziendaTrasportatoriDto>, IAziendeTrasportatoriService
    {

        #region Fields

        private IRepository<PrelievoLatte, int> prelieviRepository;
        private IRepository<Allevamento, int> allevamentiRepository;

        private IUtentiService utentiService;

        #endregion

        #region Constructors

        public AziendeTrasportatoriService(IUnitOfWork uow, IUtentiService utentiService)
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
                .ToList();

            return dropdown;
        }


        private IQueryable<AziendaTrasportatori> GetQuery(int? idUtente = null)
        {
            var query = this.repository.DbSet;

            if (!idUtente.HasValue)
                return query;

            var utente = this.utentiService.Details(idUtente.Value);
            var trasportatoriIds = new List<int?>();

            switch (utente.IdProfilo)
            {
                case 1:     // Admin
                    return query;

                case 7:     // Acquirente
                    trasportatoriIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdAcquirente == utente.IdAcquirente)
                        .Select(p => p.IdTrasportatore)
                        .Distinct()
                        .ToList();
                    return query.Where(a => trasportatoriIds.Contains(a.Id));

                case 3:     // Allevatore
                    var allevamentiIds = this.allevamentiRepository.DbSet
                        .Where(a => a.IdUtente == idUtente.Value)
                        .Select(a => a.Id).ToList();

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
                    return query.Where(a => a.Id == utente.IdAziendaTrasporti);

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


        protected override AziendaTrasportatori UpdateProperties(AziendaTrasportatori viewEntity, AziendaTrasportatori dbEntity)
        {
            dbEntity.Nome = viewEntity.Nome;
            dbEntity.Cognome = viewEntity.Cognome;
            dbEntity.P_IVA = viewEntity.P_IVA;
            dbEntity.RagioneSociale = viewEntity.RagioneSociale;

            return dbEntity;
        }

        #endregion
    }
}
