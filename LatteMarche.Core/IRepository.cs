using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LatteMarche.Core
{
	/// <summary>
	/// Interfaccia base per i repository di accesso al database
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TPrimaryKey"></typeparam>
	public interface IRepository<TEntity, TPrimaryKey>
		where TEntity : class
	{

        TEntity Add(TEntity entity);
		void Add(IEnumerable<TEntity> entities);
		void Update(TEntity entity);
		void Update(IEnumerable<TEntity> entities);
		void Delete(params TPrimaryKey[] keys);
		void Delete(TEntity entity);
		void Delete(IEnumerable<TEntity> entities);
		TEntity GetById(TPrimaryKey key);
		TEntity FindBy(Expression<Func<TEntity, bool>> expression);
		IQueryable<TEntity> GetAll();
		IQueryable<TEntity> FilterBy(Expression<Func<TEntity, bool>> where);

	}
}
