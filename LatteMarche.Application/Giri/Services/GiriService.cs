using System;

using LatteMarche.Application.Giri.Dtos;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;



namespace LatteMarche.Application.Giri.Services
{

    public class GiriService: EntityService<Giro, int, GiroDto>, IGiriService
    {

        private IRepository<Giro, int> giriRepository;

        public GiriService(IUnitOfWork uow)
            : base(uow)
        {
            this.giriRepository = this.uow.Get<Giro, int>();
        }

        public List<GiroDto> GetGiriOfTrasportatore(int idTrasportatore)
        {
            return ConvertToDtoList(this.giriRepository.FilterBy(g => g.IdTrasportatore == idTrasportatore).ToList());
        }

        protected override Giro UpdateProperties(Giro viewEntity, Giro dbEntity)
        {
            throw new NotImplementedException();
        }
    }

}
