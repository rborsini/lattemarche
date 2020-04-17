using LatteMarche.Application.Auth.Dtos;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;



namespace LatteMarche.Application.Auth.Services
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
