using System;

using LatteMarche.Application.TipiLatte.Dtos;
using LatteMarche.Application.TipiLatte.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;



namespace LatteMarche.Application.TipiLatte.Services
{

    public class TipiLatteService: EntityReadOnlyService<TipoLatte, int, TipoLatteDto>, ITipiLatteService
    {

      //  private IRepository<Tipo, int> comuniRepository;

        public TipiLatteService(IUnitOfWork uow)
            : base(uow)
        {
           // this.comuniRepository = this.uow.Get<Comune, int>();
        }
      
        public override TipoLatteDto Details(int key)
        {
            TipoLatteDto tipoLatte = base.Details(key);

            return tipoLatte;
        }
    }

}
