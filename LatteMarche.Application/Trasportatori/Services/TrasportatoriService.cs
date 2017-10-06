using System;

using LatteMarche.Application.Trasportatori.Dtos;
using LatteMarche.Application.Trasportatori.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using RB.Hash;
using LatteMarche.Application.Giri.Interfaces;
using System.Collections.Generic;

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

        public override TrasportatoreDto Details(int key)
        {
            TrasportatoreDto trasportatore = base.Details(key);

            if(trasportatore != null)
                trasportatore.Giri = this.giriService.GetGiriOfTrasportatore(key);

            return trasportatore;
        }

        #endregion
    }

}
