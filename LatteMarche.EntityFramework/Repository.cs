using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LatteMarche.Core;

namespace LatteMarche.EntityFramework
{
	/// <summary>
	/// Implementazione base del repository per l'accesso al database
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TPrimaryKey"></typeparam>
	public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>
	{
		protected IContext dataContext;
		protected readonly IDbSet<TEntity> dbset;

		public Repository(IContext context)
		{
			this.dataContext = context;
			dbset = ((DbContext)dataContext).Set<TEntity>();
		}

		public void Add(IEnumerable<TEntity> entities)
		{
			foreach (TEntity item in entities)
			{
				Add(item);
			}
		}

		public void Add(TEntity entity)
		{
			dbset.Add(entity);
		}

		public void Delete(IEnumerable<TEntity> entities)
		{
			TEntity[] entitiesArray = entities.ToArray();
			for (int i = 0; i < entitiesArray.Count(); i++)
			{
				Delete(entitiesArray[i]);
			}
		}

		public void Delete(TEntity entity)
		{
			dbset.Remove(entity);
		}

		public void Delete(params TPrimaryKey[] keys)
		{
			foreach (TPrimaryKey key in keys)
			{
				Delete(GetById(key));
			}
		}

		public IQueryable<TEntity> FilterBy(Expression<Func<TEntity, bool>> where)
		{
			return dbset.Where(where);
		}

		public TEntity FindBy(Expression<Func<TEntity, bool>> expression)
		{
			return FilterBy(expression).SingleOrDefault();
		}

		public IQueryable<TEntity> GetAll()
		{
			return dbset.AsQueryable<TEntity>();
		}

		public TEntity GetById(TPrimaryKey key)
		{
			return dbset.Find(key);
		}

		public void Update(IEnumerable<TEntity> entities)
		{
			foreach (TEntity item in entities)
			{
				Update(item);
			}
		}

		public void Update(TEntity entity)
		{
			((DbContext)dataContext).Entry(entity).State = EntityState.Modified;
		}
	}
}