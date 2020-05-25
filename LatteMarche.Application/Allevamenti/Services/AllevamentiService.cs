using System;
using System.Collections.Generic;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Linq;
using LatteMarche.Application.Comuni.Interfaces;
using AutoMapper;
using WeCode.Data.Interfaces;
using WeCode.Application;
using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.Utenti.Interfaces;

namespace LatteMarche.Application.Allevamenti.Services
{

    public class AllevamentiService : IAllevamentiService
    {

        #region Fields

        private IUnitOfWork uow;

        private IRepository<Allevamento, int> repository;
        private IRepository<PrelievoLatte, int> prelieviRepository;

        private IUtentiService utentiService;

        #endregion

        #region Constructors

        public AllevamentiService(IUnitOfWork uow, IUtentiService utentiService)
        {
            this.repository = this.uow.Get<Allevamento, int>();
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
                    Text = c.Utente.RagioneSociale
                })
                .ToList();

            return dropdown;
        }

        private IQueryable<Allevamento> GetQuery(int? idUtente = null)
        {
            var query = this.repository.DbSet;

            if (!idUtente.HasValue)
                return query;

            var utente = this.utentiService.Details(idUtente.Value);
            var allevamentiIds = new List<int?>();

            switch (utente.IdProfilo)
            {
                case 1:     // Admin
                    return query;

                case 7:     // Acquirente
                    allevamentiIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdAcquirente == utente.IdAcquirente)
                        .Select(p => p.IdAllevamento)
                        .Distinct()
                        .ToList();
                    return query.Where(a => allevamentiIds.Contains(a.Id));

                case 3:     // Allevamento
                    return query.Where(a => a.IdUtente == idUtente);

                case 4:     // Laboratorio
                    allevamentiIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdLabAnalisi == idUtente.Value)
                        .Select(p => p.IdAllevamento)
                        .Distinct()
                        .ToList();
                    return query.Where(a => allevamentiIds.Contains(a.Id));

                case 5:     // Trasportatore
                    allevamentiIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdTrasportatore == idUtente.Value)
                        .Select(p => p.IdAllevamento)
                        .Distinct()
                        .ToList();
                    return query.Where(a => allevamentiIds.Contains(a.Id));

                case 6:     // Destinatario
                    allevamentiIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdDestinatario == utente.IdDestinatario)
                        .Select(p => p.IdAllevamento)
                        .Distinct()
                        .ToList();
                    return query.Where(a => allevamentiIds.Contains(a.Id));

                case 8:     // Cessionario
                    allevamentiIds = this.prelieviRepository.DbSet
                        .Where(p => p.IdCessionario == utente.IdCessionario)
                        .Select(p => p.IdAllevamento)
                        .Distinct()
                        .ToList();
                    return query.Where(a => allevamentiIds.Contains(a.Id));

                default:
                    return query;

            }


        }

        #endregion
    }

}
