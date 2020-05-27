using System;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using LatteMarche.Application.Logs.Interfaces;
using LatteMarche.Common;
using WeCode.Application;
using WeCode.Data.Interfaces;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti.Interfaces;

namespace LatteMarche.Application.Latte.Services
{

    public class PrelieviLatteService : EntityService<PrelievoLatte, int, PrelievoLatteDto>, IPrelieviLatteService
    {

        #region Fields

        private IRepository<V_PrelievoLatte, int> v_prelieviLatteRepository;
        private IRepository<Allevamento, int> allevamentiRepository;
        private IUtentiService utentiService;

        private ILogsService logsService;

        #endregion

        #region Constructor

        public PrelieviLatteService(IUnitOfWork uow, ILogsService logsService, IUtentiService utentiService)
            : base(uow)
        {
            this.v_prelieviLatteRepository = this.uow.Get<V_PrelievoLatte, int>();
            this.allevamentiRepository = uow.Get<Allevamento, int>();

            this.logsService = logsService;
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        public IQueryable<V_PrelievoLatte> PrelieviAutorizzati(int idUtente)
        {
            var query = this.v_prelieviLatteRepository.DbSet;
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
                    return query.Where(a => a.IdCessionario == utente.IdCessionario);

                default:
                    return query;

            }
        }

        public override PrelievoLatteDto Create(PrelievoLatteDto model)
        {
            // conversione da dto a entity
            var dbEntity = ConvertToEntity(model);

            dbEntity.LastOperation = OperationEnum.Added;
            dbEntity.LastChange = DateTime.Now;

            dbEntity = this.repository.Add(dbEntity);
            this.uow.SaveChanges();

            return this.ConvertToDto(dbEntity);
        }

        public override PrelievoLatteDto Update(PrelievoLatteDto model)
        {
            var viewEntity = ConvertToEntity(model);
            var dbEntity = this.repository.GetById(viewEntity.Id);

            var entityToSave = UpdateProperties(viewEntity, dbEntity);

            dbEntity.LastOperation = OperationEnum.Updated;
            dbEntity.LastChange = DateTime.Now;

            this.repository.Update(entityToSave);
            this.uow.SaveChanges();
            return this.ConvertToDto(dbEntity);
        }

        /// <summary>
        /// Pull prelievi per sincronizzazione
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public List<PrelievoLatte> Pull(DateTime timestamp)
        {
            List<PrelievoLatte> result = new List<PrelievoLatte>();

            var query = this.repository.Query.Where(p => p.LastOperation != OperationEnum.Synched);

            query = query.Where(p => p.LastChange > timestamp);

            var entities = query.ToList();
    
            foreach (var entity in entities)
            {
                result.Add(entity.Clone() as PrelievoLatte);
                entity.LastOperation = OperationEnum.Synched;
                this.repository.Update(entity);
            }

            this.uow.SaveChanges();

            return result;
        }

        /// <summary>
        /// Push prelievi per sincronizzazione
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<PrelievoLatte> Push(List<PrelievoLatte> list)
        {
            List<PrelievoLatte> nuoviPrelievi = new List<PrelievoLatte>();

            int counter = 0;
            int tot = list.Count;
            foreach (PrelievoLatte item in list)
            {
                try
                {
                    var prelievoDb = base.repository.Query.FirstOrDefault(p =>
                            p.IdAllevamento == item.IdAllevamento &&
                            p.IdTrasportatore == item.IdTrasportatore &&
                            p.DataPrelievo == item.DataPrelievo);


                    if (prelievoDb != null)
                    {
                        // update
                        string codiceSitra = prelievoDb.CodiceSitra;
                        prelievoDb = UpdateProperties(item, prelievoDb);
                        prelievoDb.CodiceSitra = codiceSitra;
                        prelievoDb.LastChange = DateTime.Now;
                        prelievoDb.LastOperation = OperationEnum.Synched;

                        this.repository.Update(prelievoDb);
                    }
                    else
                    {
                        // insert
                        prelievoDb = item;
                        prelievoDb.LastChange = DateTime.Now;
                        prelievoDb.LastOperation = OperationEnum.Synched;

                        nuoviPrelievi.Add(item);
                        this.repository.Add(prelievoDb);
                    }

                    if(counter % Convert.ToInt32(ConfigurationManager.AppSettings["range_synch"]) == 0)
                        this.uow.SaveChanges();

                    counter++;
                }
                catch (Exception exc)
                {
                    Console.WriteLine($"Errore push prelievi ({exc.Message})");
                    // TODO: Aggiungere log
                }

            }

            this.uow.SaveChanges();

            return nuoviPrelievi;
        }

        /// <summary>
        /// Ricerca prelievi latte
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        public List<V_PrelievoLatte> Search(PrelieviLatteSearchDto searchDto, int idUtente)
        {
            IQueryable<V_PrelievoLatte> query = PrelieviAutorizzati(idUtente);

            // Allevamento
            if (searchDto.IdAllevamento.HasValue && searchDto.IdAllevamento.Value != 0)
            {
                query = query.Where(p => p.IdAllevamento == searchDto.IdAllevamento);
            }

            // Trasportatore
            if (searchDto.IdTrasportatore.HasValue && searchDto.IdTrasportatore.Value != 0)
            {
                query = query.Where(p => p.IdTrasportatore == searchDto.IdTrasportatore);
            }

            // Acquirente
            if (searchDto.IdAcquirente.HasValue && searchDto.IdAcquirente.Value != 0)
            {
                query = query.Where(p => p.IdAcquirente == searchDto.IdAcquirente);
            }

            // Cessionario
            if (searchDto.IdCessionario.HasValue && searchDto.IdCessionario.Value != 0)
            {
                query = query.Where(p => p.IdCessionario == searchDto.IdCessionario);
            }

            // Destinatario
            if (searchDto.IdDestinatario.HasValue && searchDto.IdDestinatario.Value != 0)
            {
                query = query.Where(p => p.IdDestinatario == searchDto.IdDestinatario);
            }

            // Tipo Latte
            if (searchDto.IdTipoLatte.HasValue && searchDto.IdTipoLatte.Value != 0)
            {
                query = query.Where(p => p.IdTipoLatte == searchDto.IdTipoLatte);
            }

            // Periodo Prelievo
            if (searchDto.DataPeriodoInizio.HasValue || searchDto.DataPeriodoFine.HasValue)
            {
                DateTime from = searchDto.DataPeriodoInizio.HasValue ? searchDto.DataPeriodoInizio.Value : DateTime.MinValue;
                DateTime to = searchDto.DataPeriodoFine.HasValue ? searchDto.DataPeriodoFine.Value.AddDays(1) : DateTime.MaxValue;

                query = query.Where(p => from <= p.DataPrelievo && p.DataPrelievo < to);
            }

            // Inviato sitra
            if (searchDto.InviatoSitra.HasValue)
            {
                if(searchDto.InviatoSitra.Value)
                    query = query.Where(p => !String.IsNullOrEmpty(p.CodiceSitra));
                else
                    query = query.Where(p => String.IsNullOrEmpty(p.CodiceSitra));
            }

            query = query.OrderBy(r => r.Allevamento);

            return query.ToList();
        }

        /// <summary>
        /// Ricerca prelievi latte
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        public List<V_PrelievoLatte> Sitra(DateTime data)
        {
            IQueryable<V_PrelievoLatte> query = this.v_prelieviLatteRepository.DbSet;

            DateTime from = data;
            DateTime to = data.AddDays(1);

            query = query.Where(p => from <= p.DataPrelievo && p.DataPrelievo < to);

            query = query.OrderBy(r => r.Allevamento);

            return query.ToList();
        }

        protected override PrelievoLatte UpdateProperties(PrelievoLatte viewEntity, PrelievoLatte dbEntity)
        {
            dbEntity.IdDestinatario = viewEntity.IdDestinatario;
            dbEntity.IdTrasportatore = viewEntity.IdTrasportatore;
            dbEntity.IdAcquirente = viewEntity.IdAcquirente;
            dbEntity.IdLabAnalisi = viewEntity.IdLabAnalisi;
            dbEntity.DataConsegna = viewEntity.DataConsegna;
            dbEntity.DataPrelievo = viewEntity.DataPrelievo;
            dbEntity.DataUltimaMungitura = viewEntity.DataUltimaMungitura;
            dbEntity.Quantita = viewEntity.Quantita;
            dbEntity.Temperatura = viewEntity.Temperatura;
            dbEntity.NumeroMungiture = viewEntity.NumeroMungiture;
            dbEntity.Scomparto = viewEntity.Scomparto;
            dbEntity.LottoConsegna = viewEntity.LottoConsegna;
            dbEntity.SerialeLabAnalisi = viewEntity.SerialeLabAnalisi;
            dbEntity.CodiceSitra = viewEntity.CodiceSitra;

            return dbEntity;
        }


        #endregion

    }

}
