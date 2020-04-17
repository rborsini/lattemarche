using LatteMarche.Application.Latte.Dtos;
using LatteMarche.Application.Latte.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Latte.Services
{

    public class TipiLatteService: EntityService<TipoLatte, int, TipoLatteDto>, ITipiLatteService
    {

        #region Constructors

        public TipiLatteService(IUnitOfWork uow)
            : base(uow) { }

        protected override TipoLatte UpdateProperties(TipoLatte viewEntity, TipoLatte dbEntity)
        {
            dbEntity.Descrizione = viewEntity.Descrizione;
            dbEntity.DescrizioneBreve = viewEntity.DescrizioneBreve;
            dbEntity.FattoreConversione = viewEntity.FattoreConversione;
            dbEntity.FlagInvioSitra = viewEntity.FlagInvioSitra; 

            return dbEntity;
        }

        #endregion

    }

}
