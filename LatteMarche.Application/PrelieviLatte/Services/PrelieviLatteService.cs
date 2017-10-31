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
