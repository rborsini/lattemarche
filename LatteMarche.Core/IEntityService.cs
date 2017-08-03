using System;
namespace LatteMarche.Core
{
	/// <summary>
	/// Interfaccia base per i servizi delle entità. Estende il servizio read-only con i metodi di scrittura (Create, Update, Delete)
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TPrimaryKey"></typeparam>
	/// <typeparam name="TDto"></typeparam>
	public interface IEntityService<TEntity, TPrimaryKey, TDto> : IEntityReadOnlyService<TEntity, TPrimaryKey, TDto>
		where TEntity : Entity<TPrimaryKey>
		where TDto : IEntityDto
	{

		/// <summary>
		/// Creazione di un nuovo record
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		TDto Create(TDto model);

		/// <summary>
		/// Aggiornamento di un record
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		TDto Update(TDto model);

		/// <summary>
		/// Eliminazione di un record
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		void Delete(TPrimaryKey key);


	}
}
