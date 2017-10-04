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

        #region Constructors

        public TipiLatteService(IUnitOfWork uow)
            : base(uow) { }

        #endregion

    }

}
