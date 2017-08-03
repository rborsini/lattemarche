using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LatteMarche.Core
{
	/// <summary>
	/// Interfaccia base per i servizi di tipo read only con i soli metodi di lettura (Index e  Details)
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TPrimaryKey"></typeparam>
	/// <typeparam name="TDto"></typeparam>
	public interface IEntityReadOnlyService<TEntity, TPrimaryKey, TDto>
		where TEntity : Entity<TPrimaryKey>
		where TDto : IEntityDto
	{

		/// <summary>
		/// Ritorna tutti gli elementi dell'entità corrente
		/// </summary>
		/// <returns></returns>
		List<TDto> Index();

		/// <summary>
		/// Ritorna un singolo record dell'entità, ricercato per chiave primaria
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		TDto Details(TPrimaryKey key);
	}
}
