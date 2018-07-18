using System;
using System.Collections.Generic;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Linq;

namespace LatteMarche.Application.Allevamenti.Services
{

    public class AllevamentiService : EntityService<Allevamento, int, AllevamentoDto>, IAllevamentiService
    {

        #region Fields

        private IRepository<Allevamento, int> allevamentiRepository;
        private IRepository<V_Allevamento, int> v_allevamentiRepository;

        #endregion

        #region Constructors

        public AllevamentiService(IUnitOfWork uow)
            : base(uow)
        {
            this.allevamentiRepository = this.uow.Get<Allevamento, int>();
            this.v_allevamentiRepository = this.uow.Get<V_Allevamento, int>();
        }

        #endregion

        #region Methods

        public List<AllevamentoDto> GetAllevamentiSitra()
        {
            return ConvertToDtoList(this.repository.FilterBy(a => !String.IsNullOrEmpty(a.CUAA)).ToList());
        }

        public decimal GetFattoreConversione(int idAllevamento)
        {
            var v_allevamento = this.v_allevamentiRepository.GetById(idAllevamento);

            return v_allevamento != null ? v_allevamento.FattoreConversione : 0;
        }

        protected override Allevamento UpdateProperties(Allevamento viewEntity, Allevamento dbEntity)
        {
            dbEntity.CodiceAsl = viewEntity.CodiceAsl;
            dbEntity.IndirizzoAllevamento = viewEntity.IndirizzoAllevamento;
            dbEntity.IdComune = viewEntity.IdComune;
            dbEntity.IdSitraStabilimentoAllevamento = viewEntity.IdSitraStabilimentoAllevamento;

            return dbEntity;
        }

        #endregion
    }

}
