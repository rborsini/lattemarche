using System;

using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;



namespace LatteMarche.Application.PrelieviLatte.Services
{

    public class PrelieviLatteService : EntityService<PrelievoLatte, int, PrelievoLatteDto>, IPrelieviLatteService
    {

        #region Fields

        private IRepository<PrelievoLatte, int> prielieviLatteRepository;

        #endregion

        #region Constructor

        public PrelieviLatteService(IUnitOfWork uow)
            : base(uow)
        {
            this.prielieviLatteRepository = this.uow.Get<PrelievoLatte, int>();
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
        public List<PrelievoLatteDto> Pull(DateTime? timestamp)
        {
            List<PrelievoLatteDto> result = new List<PrelievoLatteDto>();

            var query = this.repository.FilterBy(p => p.LastOperation != Common.OperationEnum.Synched);

            if (timestamp.HasValue)
                query = query.Where(p => p.LastChange > timestamp.Value);

            var entities = query.ToList();
            result = ConvertToDtoList(entities);

            foreach(var entity in entities)
            {
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
        public int Push(List<PrelievoLatteDto> list)
        {
            int counter = 0;
            foreach(PrelievoLatteDto item in list)
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
                        prelievoDb = UpdateProperties(ConvertToEntity(item), prelievoDb);
                        prelievoDb.LastOperation = Common.OperationEnum.Synched;

                        this.repository.Update(prelievoDb);
                    }
                    else
                    {
                        // insert
                        prelievoDb = ConvertToEntity(item);
                        prelievoDb.LastOperation = Common.OperationEnum.Synched;

                        this.repository.Add(prelievoDb);
                    }

                    this.uow.SaveChanges();
                    counter++;
                }
                catch { }

            }

            return counter;
        }

        /// <summary>
        /// Ricerca prelievi latte
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        public List<PrelievoLatteDto> Search(PrelieviLatteSearchDto searchDto)
        {
            IQueryable<PrelievoLatte> query = this.prielieviLatteRepository.GetAll();

            // Allevamento
            if (searchDto.idAllevamento != null)
            {
                query = query.Where(p => p.IdAllevamento == searchDto.idAllevamento);
            }

            // Periodo Prelievo
            if (searchDto.DataPeriodoInizio.HasValue || searchDto.DataPeriodoFine.HasValue)
            {
                DateTime from = searchDto.DataPeriodoInizio.HasValue ? searchDto.DataPeriodoInizio.Value : DateTime.MinValue;
                DateTime to = searchDto.DataPeriodoFine.HasValue ? searchDto.DataPeriodoFine.Value.AddDays(1) : DateTime.MaxValue;

                query = query.Where(p => from <= p.DataPrelievo && p.DataPrelievo < to);
            }

            return ConvertToDtoList(query.ToList());
        }

        protected override PrelievoLatte UpdateProperties(PrelievoLatte viewEntity, PrelievoLatte dbEntity)
        {
            dbEntity.IdDestinatario = viewEntity.IdDestinatario;
            dbEntity.IdTrasportatore = viewEntity.IdTrasportatore;
            dbEntity.IdAquirente = viewEntity.IdAquirente;
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

            return dbEntity;
        }


        #endregion

    }

}
