using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using LatteMarche.Application.Acquirenti.Dtos;
using LatteMarche.Application.Acquirenti.Interfaces;
using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using WeCode.Application;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Acquirenti.Services
{

    public class AcquirentiService : EntityService<Acquirente, int, AcquirenteDto>, IAcquirentiService
	{

        #region Fields

        private IRepository<Allevamento, int> allevamentiRepository;
        private IRepository<PrelievoLatte, int> prelieviRepository;

        private IUtentiService utentiService;

        #endregion

        #region Constructors

        public AcquirentiService(IUnitOfWork uow, IUtentiService utentiService)
			: base(uow)
		{
            this.allevamentiRepository = this.uow.Get<Allevamento, int>();
            this.prelieviRepository = this.uow.Get<PrelievoLatte, int>();
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        public override List<AcquirenteDto> Index()
        {
            var entities = this.repository.DbSet.Where(d => d.Abilitato).ToList();

            return Mapper.Map<List<AcquirenteDto>>(entities);
        }

        public DropDownDto DropDown(int? idUtente = null)
        {
            var dropdown = new DropDownDto();

            var query = GetQuery(idUtente);

            dropdown.Items = query
                .Where(c => c.Abilitato)
                .Select(c => new DropDownItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.RagioneSociale
                })
                .OrderBy(i => i.Text)
                .ToList();

            return dropdown;
        }

        private IQueryable<Acquirente> GetQuery(int? idUtente = null)
        {
            var query = this.repository.DbSet;

            if (!idUtente.HasValue)
                return query;

            var utente = this.utentiService.Details(idUtente.Value);
            var acquirentiIds = new List<int?>();

            switch(utente.IdProfilo)
            {
                case 1:     // Admin
                    return query;

                case 7:     // Acquirente
                    return query.Where(a => a.Id == utente.IdAcquirente);

                case 3:
                    // allevamenti associati all'utente
                    var allevamentiIds = this.allevamentiRepository.DbSet
                        .Where(a => a.IdUtente == idUtente.Value)
                        .Select(a => a.Id).ToList();

                    // acquirenti associati a prelievi effettuati presso gli allevamenti relazionati
                    acquirentiIds = this.prelieviRepository.DbSet
                        .Where(p => allevamentiIds.Contains(p.IdAllevamento.Value))
                        .Select(p => p.IdAcquirente)
                        .Distinct()
                        .ToList();
                    return query.Where(a => acquirentiIds.Contains(a.Id));

                case 4:     // Laboratorio
                    acquirentiIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdLabAnalisi ==  idUtente.Value)
                        .Select(p => p.IdAcquirente)
                        .Distinct()
                        .ToList();
                    return query.Where(a => acquirentiIds.Contains(a.Id));

                case 5:     // Trasportatore
                    acquirentiIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdTrasportatore == idUtente.Value)
                        .Select(p => p.IdAcquirente)
                        .Distinct()
                        .ToList();
                    return query.Where(a => acquirentiIds.Contains(a.Id));

                case 6:     // Destinatario
                    acquirentiIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdDestinatario == utente.IdDestinatario)
                        .Select(p => p.IdAcquirente)
                        .Distinct()
                        .ToList();
                    return query.Where(a => acquirentiIds.Contains(a.Id));

                case 8:     // Cessionario
                    acquirentiIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdCessionario == utente.IdCessionario)
                        .Select(p => p.IdAcquirente)
                        .Distinct()
                        .ToList();
                    return query.Where(a => acquirentiIds.Contains(a.Id));

                default:
                    return query;
                
            }


        }

        protected override Acquirente UpdateProperties(Acquirente viewEntity, Acquirente dbEntity)
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
