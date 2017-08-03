using System;

using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Utenti.Services
{

    public class UtentiService : EntityService<Utente, int, UtenteDto>, IUtentiService
	{

		#region Fields

		private IRepository<Utente, int> utentiRepository;			

		#endregion

		#region Constructors

		public UtentiService(IUnitOfWork uow)
			: base(uow)
		{
			this.utentiRepository = this.uow.Get<Utente, int>();
		}

		#endregion

		#region Methods

		protected override Utente UpdateProperties(Utente viewEntity, Utente dbEntity)
		{
            dbEntity.Nome = viewEntity.Nome;
            dbEntity.Cognome = viewEntity.Cognome;

			return dbEntity;
		}

		#endregion
	}

}
