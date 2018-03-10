using System;

using LatteMarche.Application.TipiLatte.Dtos;
using LatteMarche.Application.TipiLatte.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;



namespace LatteMarche.Application.TipiLatte.Services
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
