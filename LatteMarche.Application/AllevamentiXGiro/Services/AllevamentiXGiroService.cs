using System;

using LatteMarche.Application.AllevamentiXGiro.Dtos;
using LatteMarche.Application.AllevamentiXGiro.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;



namespace LatteMarche.Application.AllevamentiXGiro.Services
{

    public class AllevamentiXGiroService : EntityService<AllevamentoXGiro, int, AllevamentoXGiroDto>, IAllevamentiXGiroService
    {
        #region Fields

        private IRepository<AllevamentoXGiro, int> allevamentiXGiroRepository;

        #endregion

        #region Constructors

        public AllevamentiXGiroService(IUnitOfWork uow)
            : base(uow)
        {
            this.allevamentiXGiroRepository = this.uow.Get<AllevamentoXGiro, int>();
        }

        #endregion

        #region Methods

        public List<AllevamentoXGiroDto> GetByGiro(int idGiro)
        {
           return ConvertToDtoList(this.allevamentiXGiroRepository.FilterBy(a => a.IdGiro == idGiro).ToList());

        }

        protected override AllevamentoXGiro UpdateProperties(AllevamentoXGiro viewEntity, AllevamentoXGiro dbEntity)
        {
            throw new NotImplementedException();
        }
        #endregion

    }

}
