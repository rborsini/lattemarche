using System;

using LatteMarche.Application.VPrelieviLatte.Dtos;
using LatteMarche.Application.VPrelieviLatte.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;



namespace LatteMarche.Application.VPrelieviLatte.Services
{

    public class VPrelieviLatteService : EntityReadOnlyService<V_PrelievoLatte, int, VPrelievoLatteDto>, IVPrelieviLatteService
    {

        #region Fields

        private IRepository<V_PrelievoLatte, int> vPrielieviLatteRepository;

        #endregion

        #region Constructor

        public VPrelieviLatteService(IUnitOfWork uow)
            : base(uow)
        {
            this.vPrielieviLatteRepository = this.uow.Get<V_PrelievoLatte, int>();
        }

        #endregion

        #region Methods

        public List<VPrelievoLatteDto> getPrelieviByIdAllevamento(int idAllevamento)
        {
            return ConvertToDtoList(this.vPrielieviLatteRepository.FilterBy(p => p.IdAllevamento == idAllevamento).ToList());
        }
   
        #endregion

    }

}
