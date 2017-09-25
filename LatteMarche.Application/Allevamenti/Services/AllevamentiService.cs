using System;

using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using RB.Hash;

namespace LatteMarche.Application.Allevamenti.Services
{

    public class AllevamentiService : EntityService<Allevamento, int, AllevamentoDto>, IAllevamentiService
	{

		#region Fields

		private IRepository<Allevamento, int> allevamentiRepository;			

		#endregion

		#region Constructors

		public AllevamentiService(IUnitOfWork uow)
			: base(uow)
		{
			this.allevamentiRepository = this.uow.Get<Allevamento, int>();
		}

        #endregion

        #region Methods

        public override AllevamentoDto Details(int key)
        {
            AllevamentoDto allevamentoDto = null;
            Allevamento allevamento = this.allevamentiRepository.GetById(key);
            if (allevamento != null)
            {
                allevamentoDto = ConvertToDto(allevamento);
            }

            return allevamentoDto;
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
