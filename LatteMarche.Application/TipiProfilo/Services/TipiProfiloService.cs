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

        private IRepository<TipoProfilo, int> tipiProfiloRepository;

        #endregion

        #region Constructors

        public TipiProfiloService(IUnitOfWork uow)
            : base(uow)
        {
            this.tipiProfiloRepository = this.uow.Get<TipoProfilo, int>();
        }

        #endregion

        #region Methods

        public int GetIdProfilo(string DescrizioneProfilo)
        {
            return this.tipiProfiloRepository.FindBy(t => t.Descrizione == DescrizioneProfilo).Id;
        }

        #endregion

    }

}
