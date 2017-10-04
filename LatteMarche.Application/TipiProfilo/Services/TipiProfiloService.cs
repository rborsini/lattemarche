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

        #region Fields

        #endregion

        #region Constructors

        public TipiProfiloService(IUnitOfWork uow)
            : base(uow) { }

        #endregion

        #region Methods

        #endregion

    }

}
