using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LatteMarche.Core;

namespace LatteMarche.EntityFramework
{

	/// <summary>
	/// Implementazione base UnitOfWork
	/// </summary>
	public class UnitOfWork : IUnitOfWork
	{

		private IContext context;
		private ObjectContext objectContext;
		private IDbTransaction transaction;
		private bool disposed;
		private Object objectLock = new Object();
		private Hashtable repositories;

        public IContext Context { get { return this.context; } }

        public UnitOfWork(IContext context)
		{
			this.context = context;
			this.repositories = new Hashtable();
		}

		public void SaveChanges()
		{
			try
			{
				this.context.SaveChanges();
			}
			catch (DbEntityValidationException exc)
			{
				// Retrieve the error messages as a list of strings.
				var errorMessages = exc.EntityValidationErrors
						.SelectMany(x => x.ValidationErrors)
						.Select(x => x.ErrorMessage);

				// Join the list to a single string.
				var fullErrorMessage = string.Join("; ", errorMessages);

				// Combine the original exception message with the new one.
				var exceptionMessage = string.Concat(exc.Message, " The validation errors are: ", fullErrorMessage);
				throw new Exception(exceptionMessage, exc);
			}
		}

		public IDbTransaction BeginTransaction()
		{
			this.objectContext = ((IObjectContextAdapter)this.context).ObjectContext;
			if (this.objectContext.Connection.State != ConnectionState.Open)
			{
				this.objectContext.Connection.Open();
			}

			this.transaction = this.objectContext.Connection.BeginTransaction(IsolationLevel.Unspecified);
			return this.transaction;
		}

		public void Commit()
		{
			this.transaction.Commit();
		}

		public void Rollback()
		{
			this.transaction.Rollback();
		}

		public IRepository<TEntity, TPrimaryKey> Get<TEntity, TPrimaryKey>() where TEntity : Entity<TPrimaryKey>
		{
			string type = typeof(TEntity).Name;

			lock (objectLock)
			{
				if (!this.repositories.ContainsKey(type))
				{
					this.repositories.Add(type, new Repository<TEntity, TPrimaryKey>(this.context));
				}
			}

			return (IRepository<TEntity, TPrimaryKey>)this.repositories[type];
		}

		public void Dispose()
		{
			Dispose(true);
		}

		public virtual void Dispose(bool disposing)
		{
			if (this.disposed)
				return;

			if (disposing)
			{
				// free other managed objects that implement
				// IDisposable only

				try
				{
					if (this.objectContext != null && this.objectContext.Connection.State == ConnectionState.Open)
					{
						this.objectContext.Connection.Close();
					}
				}
				catch (ObjectDisposedException)
				{
					// do nothing, the objectContext has already been disposed
				}

				if (this.objectContext != null)
				{
					this.objectContext.Dispose();
					this.objectContext = null;
				}
			}

			// release any unmanaged objects
			// set the object references to null

			this.disposed = true;
		}
	}
}
