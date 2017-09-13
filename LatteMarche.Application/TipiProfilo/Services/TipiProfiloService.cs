using System;

using LatteMarche.Application.TipiProfilo.Dtos;
using LatteMarche.Application.TipiProfilo.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;



namespace LatteMarche.Application.TipiProfilo.Services
{

    public class TipiProfiloService: EntityReadOnlyService<TipoProfilo, int, TipoProfiloDto>, ITipiProfiloService
    {

      //  private IRepository<Tipo, int> comuniRepository;

        public TipiProfiloService(IUnitOfWork uow)
            : base(uow)
        {
           // this.comuniRepository = this.uow.Get<Comune, int>();
        }
      
        public override TipoProfiloDto Details(int key)
        {
            TipoProfiloDto tipoProfilo = base.Details(key);

            return tipoProfilo;
        }
    }

}
