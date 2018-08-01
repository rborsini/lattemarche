using System;
using System.Collections.Generic;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Linq;
using LatteMarche.Application.Comuni.Interfaces;

namespace LatteMarche.Application.Allevamenti.Services
{

    public class AllevamentiService : EntityService<Allevamento, int, AllevamentoDto>, IAllevamentiService
    {

        #region Fields

        private IRepository<Allevamento, int> allevamentiRepository;
        private IRepository<V_Allevamento, int> v_allevamentiRepository;

        private IComuniService comuniService;

        #endregion

        #region Constructors

        public AllevamentiService(IUnitOfWork uow, IComuniService comuniService)
            : base(uow)
        {
            this.allevamentiRepository = this.uow.Get<Allevamento, int>();
            this.v_allevamentiRepository = this.uow.Get<V_Allevamento, int>();

            this.comuniService = comuniService;
        }

        #endregion

        #region Methods

        public List<AllevamentoDto> GetAllevamentiSitra()
        {
            return ConvertToDtoList(this.repository.FilterBy(a => !String.IsNullOrEmpty(a.CUAA)).ToList());
        }

        public List<V_Allevamento> Search()
        {
            return this.v_allevamentiRepository.GetAll().ToList();
        }

        public override AllevamentoDto Details(int key)
        {
            var dto = base.Details(key);

            if (dto != null && dto.IdComune != 0)
            {
                var comuneObj = this.comuniService.Details(dto.IdComune);

                if (comuneObj != null)
                    dto.SiglaProvincia = comuneObj.Provincia;
            }

            return dto;
        }

        protected override Allevamento UpdateProperties(Allevamento viewEntity, Allevamento dbEntity)
        {
            dbEntity.CodiceAsl = viewEntity.CodiceAsl;
            dbEntity.IndirizzoAllevamento = viewEntity.IndirizzoAllevamento;
            dbEntity.IdComune = viewEntity.IdComune;
            dbEntity.CUAA = viewEntity.CUAA;

            return dbEntity;
        }

        #endregion
    }

}
