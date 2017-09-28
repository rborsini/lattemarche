using System;

using LatteMarche.Application.Trasportatori.Dtos;
using LatteMarche.Application.Trasportatori.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using RB.Hash;

namespace LatteMarche.Application.Trasportatori.Services
{

    public class TrasportatoriService : EntityReadOnlyService<V_Trasportatore, int, TrasportatoreDto>, ITrasportatoriService
	{

		#region Fields
         
		private IRepository<Utente, int> trasportatoriRepository;			

		#endregion

		#region Constructors

		public TrasportatoriService(IUnitOfWork uow)
			: base(uow)
		{
			this.trasportatoriRepository = this.uow.Get<Utente, int>();
		}

        #endregion

        #region Methods
     
        #endregion
    }

}
