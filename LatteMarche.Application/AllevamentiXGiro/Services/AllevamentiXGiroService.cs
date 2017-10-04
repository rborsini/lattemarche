using System;

using LatteMarche.Application.AllevamentiXGiro.Dtos;
using LatteMarche.Application.AllevamentiXGiro.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;



namespace LatteMarche.Application.AllevamentiXGiro.Services
{

    public class AllevamentiXGiroService : EntityReadOnlyService<V_AllevamentoXGiro, int, AllevamentoXGiroDto>, IAllevamentiXGiroService
    {
        #region Fields

        private IRepository<V_AllevamentoXGiro, int> allevamentiXGiroRepository;

        #endregion

        #region Constructors

        public AllevamentiXGiroService(IUnitOfWork uow)
            : base(uow)
        {
            this.allevamentiXGiroRepository = this.uow.Get<V_AllevamentoXGiro, int>();
        }

        #endregion

        #region Methods

        public List<AllevamentoXGiroDto> GetByGiro(int idGiro)
        {
           return ConvertToDtoList(this.allevamentiXGiroRepository.FilterBy(a => a.IdGiro == idGiro).ToList());

        }
        #endregion

    }

}
