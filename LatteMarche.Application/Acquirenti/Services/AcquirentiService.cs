using System;

using LatteMarche.Application.Acquirenti.Dtos;
using LatteMarche.Application.Acquirenti.Interfaces;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Acquirenti.Services
{

    public class AcquirentiService : EntityService<Acquirente, int, AcquirenteDto>, IAcquirentiService
	{

		#region Fields

		private IRepository<Acquirente, int> acquirentiRepository;
        private IRepository<UtenteXAcquirente, int> utentiAcquirenteRepository;

        private IComuniService comuniService;

		#endregion

		#region Constructors

		public AcquirentiService(IUnitOfWork uow, IComuniService comuniService)
			: base(uow)
		{
			this.acquirentiRepository = this.uow.Get<Acquirente, int>();
            this.utentiAcquirenteRepository = this.uow.Get<UtenteXAcquirente, int>();
            this.comuniService = comuniService;
		}

        #endregion

        #region Methods

        public override AcquirenteDto Details(int key)
        {
            var dto = base.Details(key);

            if (dto != null)
            {
                var comuneObj = this.comuniService.Details(dto.IdComune);

                if (comuneObj != null)
                    dto.SiglaProvincia = comuneObj.Provincia;
            }

            return dto;
        }

        public AcquirenteDto GetByIdUtente(long idUtente)
        {
            var utenteXAcquirente = this.utentiAcquirenteRepository.FindBy(u => u.Id == idUtente);

            if (utenteXAcquirente == null)
                return null;

            return this.Details(utenteXAcquirente.IdAcquirente);
        }

        protected override Acquirente UpdateProperties(Acquirente viewEntity, Acquirente dbEntity)
        {
            dbEntity.IdComune = viewEntity.IdComune;
            dbEntity.Indirizzo = viewEntity.Indirizzo;
            dbEntity.Piva = viewEntity.Piva;
            dbEntity.RagioneSociale = viewEntity.RagioneSociale;

            return dbEntity;
        }

        #endregion
    }

}
