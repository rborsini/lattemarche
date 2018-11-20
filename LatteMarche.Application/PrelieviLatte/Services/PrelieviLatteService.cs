using System;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using LatteMarche.Application.Logs.Interfaces;

namespace LatteMarche.Application.PrelieviLatte.Services
{

    public class PrelieviLatteService : EntityService<PrelievoLatte, int, PrelievoLatteDto>, IPrelieviLatteService
    {

        #region Fields

        private IRepository<PrelievoLatte, int> prielieviLatteRepository;
        private IRepository<V_PrelievoLatte, int> v_prelieviLatteRepository;

        private ILogsService logsService;

        #endregion

        #region Constructor

        public PrelieviLatteService(IUnitOfWork uow, ILogsService logsService)
            : base(uow)
        {
            this.prielieviLatteRepository = this.uow.Get<PrelievoLatte, int>();
            this.v_prelieviLatteRepository = this.uow.Get<V_PrelievoLatte, int>();

            this.logsService = logsService;
        }

        #endregion

        #region Methods

        public override PrelievoLatteDto Create(PrelievoLatteDto model)
        {
            // conversione da dto a entity
            var dbEntity = ConvertToEntity(model);

            dbEntity.LastOperation = Common.OperationEnum.Added;
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

            dbEntity.LastOperation = Common.OperationEnum.Updated;
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

            var query = this.repository.FilterBy(p => p.LastOperation != Common.OperationEnum.Synched);

            query = query.Where(p => p.LastChange > timestamp);

            var entities = query.ToList();
    
            foreach (var entity in entities)
            {
                result.Add(entity.Clone() as PrelievoLatte);
                entity.LastOperation = Common.OperationEnum.Synched;
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
                    var prelievoDb = repository.FindBy(p =>
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
                        prelievoDb.LastOperation = Common.OperationEnum.Synched;

                        this.repository.Update(prelievoDb);
                    }
                    else
                    {
                        // insert
                        prelievoDb = item;
                        prelievoDb.LastChange = DateTime.Now;
                        prelievoDb.LastOperation = Common.OperationEnum.Synched;

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
        public List<V_PrelievoLatte> Search(PrelieviLatteSearchDto searchDto)
        {
            IQueryable<V_PrelievoLatte> query = this.v_prelieviLatteRepository.GetAll();

            // Allevamento
            if (searchDto.idAllevamento != null)
            {
                query = query.Where(p => p.IdAllevamento == searchDto.idAllevamento);
            }

            if (searchDto.idTrasportatore != null)
            {
                query = query.Where(p => p.IdTrasportatore == searchDto.idTrasportatore);
            }

            if (searchDto.idAcquirente != null)
            {
                query = query.Where(p => p.IdAcquirente == searchDto.idAcquirente);
            }

            if (searchDto.idDestinatario != null)
            {
                query = query.Where(p => p.IdDestinatario == searchDto.idDestinatario);
            }

            if (searchDto.idTipoLatte != null)
            {
                query = query.Where(p => p.IdTipoLatte == searchDto.idTipoLatte);
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
