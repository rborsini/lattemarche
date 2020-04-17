
using LatteMarche.Application.Trasportatori.Dtos;
using LatteMarche.Application.Trasportatori.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Trasportatori.Services
{

    public class TrasportatoriService : EntityReadOnlyService<V_Trasportatore, int, TrasportatoreDto>, ITrasportatoriService
	{

		#region Fields
         
		private IRepository<Utente, int> trasportatoriRepository;

        private IGiriService giriService;

		#endregion

		#region Constructors

		public TrasportatoriService(IUnitOfWork uow, IGiriService giriService)
			: base(uow)
		{
			this.trasportatoriRepository = this.uow.Get<Utente, int>();

            this.giriService = giriService;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Caricamento dettaglio trasportatore
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override TrasportatoreDto Details(int key)
        {
            TrasportatoreDto trasportatore = base.Details(key);

            if(trasportatore != null)
                trasportatore.Giri = this.giriService.GetGiriTrasportatore(key);

            return trasportatore;
        }

        #endregion
    }

}
