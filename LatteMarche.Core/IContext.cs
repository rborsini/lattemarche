using LatteMarche.Core.Models;
using System;
using System.Data.Entity;

namespace LatteMarche.Core
{
	/// <summary>
	///  Astrazione del contesto database
	/// </summary>
	public interface IContext
	{

        DbSet<AllevamentoXGiro> AllevamentiXGiro { get; set; }

        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        void SetModified(object entity);

        /// <summary>
        /// Commit delle modifiche pendenti
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

	}
}
