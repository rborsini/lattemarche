using System;

using LatteMarche.Application.Acquirenti.Dtos;
using LatteMarche.Application.Acquirenti.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Acquirenti.Services
{

    public class AcquirentiService : EntityService<Acquirente, int, AcquirenteDto>, IAcquirentiService
	{

		#region Fields

		private IRepository<Acquirente, int> acquirentiRepository;			

		#endregion

		#region Constructors

		public AcquirentiService(IUnitOfWork uow)
			: base(uow)
		{
			this.acquirentiRepository = this.uow.Get<Acquirente, int>();
		}

        protected override Acquirente UpdateProperties(Acquirente viewEntity, Acquirente dbEntity)
        {
            //Scoprire cosa si può modificare e cosa no
            throw new NotImplementedException();
        }

        #endregion

        #region Methods

        #endregion
    }

}
