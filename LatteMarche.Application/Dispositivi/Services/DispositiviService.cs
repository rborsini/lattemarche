using LatteMarche.Application.Dispositivi.Dtos;
using LatteMarche.Application.Dispositivi.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Dispositivi.Services
{
    public class DispositiviService : EntityService<DispositivoMobile, string, DispositivoMobileDto>, IDispositiviService
    {

        #region Fields

        #endregion

        #region Constructor

        public DispositiviService(IUnitOfWork uow)
            : base(uow)
        { }

        #endregion

        #region Methods

        public override List<DispositivoMobileDto> Index()
        {
            return this.repository.GetAll().ProjectToList<DispositivoMobileDto>();
        }

        protected override DispositivoMobile UpdateProperties(DispositivoMobile viewEntity, DispositivoMobile dbEntity)
        {
            dbEntity.Attivo = viewEntity.Attivo;
            dbEntity.IdTrasportatore = viewEntity.IdTrasportatore;

            return dbEntity;
        }

        #endregion

    }
}
