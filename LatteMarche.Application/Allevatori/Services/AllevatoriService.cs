using System;

using LatteMarche.Application.Allevatori.Dtos;
using LatteMarche.Application.Allevatori.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;



namespace LatteMarche.Application.Allevatori.Services
{

    public class AllevatoriService : EntityReadOnlyService<V_Allevatore, int, AllevatoreDto>, IAllevatoriService
    {
        private IRepository<V_Allevatore, int> allevatoriRepository;

        public AllevatoriService(IUnitOfWork uow)
            : base(uow)
        {
            this.allevatoriRepository = this.uow.Get<V_Allevatore, int>();
        }


    }

}
