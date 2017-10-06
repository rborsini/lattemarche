using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LatteMarche.Core
{
	/// <summary>
	/// Interfaccia per l'identificazione di una singola unità di lavoro per la creazione di repository e commit delle modifiche
	/// </summary>
	public interface IUnitOfWork : IDisposable
	{
        IContext Context { get; }
        void SaveChanges();
		IDbTransaction BeginTransaction();
		void Commit();
		void Rollback();

		IRepository<TEntity, TPrimaryKey> Get<TEntity, TPrimaryKey>() where TEntity : Entity<TPrimaryKey>;
	}
}
