using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LatteMarche.Core;

namespace LatteMarche.Application
{

	/// <summary>
	/// Classe base per la sola lettura di entity
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TPrimaryKey"></typeparam>
	/// <typeparam name="TDto"></typeparam>
	public abstract class EntityReadOnlyService<TEntity, TPrimaryKey, TDto> : IEntityReadOnlyService<TEntity, TPrimaryKey, TDto>
		where TEntity : Entity<TPrimaryKey>
		where TDto : IEntityDto
	{

		protected IRepository<TEntity, TPrimaryKey> repository;
		protected IUnitOfWork uow;

		public EntityReadOnlyService(IUnitOfWork uow)
		{
			this.uow = uow;
			this.repository = uow.Get<TEntity, TPrimaryKey>();
		}

		/// <summary>
		/// Ritorna l'elenco di tutti gli elementi dell'entità corrente
		/// </summary>
		/// <returns></returns>
		public virtual List<TDto> Index()
		{
			return ConvertToDtoList(this.repository.GetAll().ToList());
		}

		/// <summary>
		/// Ritorna un singolo record dell'entità corrente
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public virtual TDto Details(TPrimaryKey key)
		{
			return ConvertToDto(this.repository.GetById(key));
		}

		/// <summary>
		/// Conversione da Entity a Dto
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		protected TDto ConvertToDto(TEntity entity)
		{
			return Mapper.Map<TDto>(entity);
		}

		/// <summary>
		/// Conversione di una lista di entity in una lista di Dto
		/// </summary>
		/// <param name="entities"></param>
		/// <returns></returns>
		protected List<TDto> ConvertToDtoList(List<TEntity> entities)
		{
			return Mapper.Map<List<TDto>>(entities);
		}
	}

}
