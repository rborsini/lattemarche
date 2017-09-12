using System;

using LatteMarche.Application.Comuni.Dtos;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;



namespace LatteMarche.Application.Comuni.Services
{

    public class ComuniService : EntityReadOnlyService<Comune, int, ComuneDto>, IComuniService
    {

        private IRepository<Comune, int> comuniRepository;

        public ComuniService(IUnitOfWork uow)
            : base(uow)
        {
            this.comuniRepository = this.uow.Get<Comune, int>();
        }

        public List<ComuneDto> Search(string provincia)
        {
            return ConvertToDtoList(this.repository.FilterBy(p => p.Provincia == provincia).OrderBy(c => c.Descrizione).ToList());
        }

        public List<String> getProvince()
        {
            return this.repository.GetAll().ToList().Select(p => p.Provincia).Distinct().ToList();
        }
    
        public override ComuneDto Details(int key)
        {
            ComuneDto comune = base.Details(key);

            return comune;
        }
    }

}
