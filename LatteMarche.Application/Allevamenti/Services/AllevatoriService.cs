using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Allevamenti.Interfaces;
using WeCode.Data.Interfaces;
using WeCode.Application;

namespace LatteMarche.Application.Allevamenti.Services
{

    public class AllevatoriService : EntityReadOnlyService<V_Allevatore, int, AllevatoreDto>, IAllevatoriService
    {

        #region Fields

        private IRepository<V_Allevatore, int> allevatoriRepository;

        #endregion

        #region Constructor

        public AllevatoriService(IUnitOfWork uow)
            : base(uow)
        {
            this.allevatoriRepository = this.uow.Get<V_Allevatore, int>();
        }

        #endregion

        #region Methods

        public override List<AllevatoreDto> Index()
        {
            return ConvertToDtoList(this.repository.Query.OrderBy(a => a.RagioneSociale).ToList());
        }

        #endregion

    }

}
