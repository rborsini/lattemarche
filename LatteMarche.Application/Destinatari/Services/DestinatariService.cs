using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Application.Destinatari.Dtos;
using LatteMarche.Application.Destinatari.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Application;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Destinatari.Services
{
    public class DestinatariService : EntityService<Destinatario, int, DestinatarioDto>, IDestinatariService
    {
        #region Fields

        private IRepository<UtenteXDestinatario, int> utentiDestinatarioRepository;

        private IComuniService comuniService;

        #endregion

        #region Constructors

        public DestinatariService(IUnitOfWork uow, IComuniService comuniService)
            : base(uow)
        {
            this.utentiDestinatarioRepository = this.uow.Get<UtenteXDestinatario, int>();
            this.comuniService = comuniService;
        }

        #endregion

        #region Methods

        public override DestinatarioDto Details(int key)
        {
            var dto = base.Details(key);

            if (dto != null && dto.IdComune.HasValue)
            {
                var comuneObj = this.comuniService.Details(dto.IdComune.Value);

                if (comuneObj != null)
                    dto.SiglaProvincia = comuneObj.Provincia;
            }

            return dto;
        }

        public DestinatarioDto GetByIdUtente(long idUtente)
        {
            var utenteXDestinatario = this.utentiDestinatarioRepository.Query.FirstOrDefault(u => u.Id == idUtente);

            if (utenteXDestinatario == null)
                return null;

            return this.Details(utenteXDestinatario.IdDestinatario);
        }

        protected override Destinatario UpdateProperties(Destinatario viewEntity, Destinatario dbEntity)
        {
            dbEntity.IdComune = viewEntity.IdComune;
            dbEntity.Indirizzo = viewEntity.Indirizzo;
            dbEntity.P_IVA = viewEntity.P_IVA;
            dbEntity.RagioneSociale = viewEntity.RagioneSociale;
            dbEntity.Stabilimento = viewEntity.Stabilimento;

            return dbEntity;
        }

        #endregion
    }
}
