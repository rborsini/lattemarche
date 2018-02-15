using LatteMarche.Application.Lotti.Dtos;
using LatteMarche.Application.Lotti.Interfaces;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Lotti.Services
{
    public class LottiService : EntityService<Lotto, long, LottoDto>, ILottiService
    {
        public LottiService(IUnitOfWork uow)
            : base(uow)
        { }

        public List<LottoDto> GetLotti(List<PrelievoLatte> prelievi)
        {
            throw new NotImplementedException();
        }

        protected override Lotto UpdateProperties(Lotto viewEntity, Lotto dbEntity)
        {
            return dbEntity;
        }
    }
}
