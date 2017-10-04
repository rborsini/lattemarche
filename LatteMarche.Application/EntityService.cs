using AutoMapper;
using System;
using LatteMarche.Core;

namespace LatteMarche.Application
{
	/// <summary>
	/// Classe base per la lettura e scrittura di entity (Index, Details, Create, Update, Delete)
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TPrimaryKey"></typeparam>
	/// <typeparam name="TDto"></typeparam>
	public abstract class EntityService<TEntity, TPrimaryKey, TDto> : EntityReadOnlyService<TEntity, TPrimaryKey, TDto>, IEntityService<TEntity, TPrimaryKey, TDto>
		where TEntity : Entity<TPrimaryKey>
		where TDto : IEntityDto
	{

		public EntityService(IUnitOfWork uow)
			: base(uow)
		{ }

		/// <summary>
		/// Creazione di un nuovo record nel database
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public virtual TDto Create(TDto model)
		{
			// conversione da dto a entity
			TEntity dbEntity = ConvertToEntity(model);

            dbEntity = this.repository.Add(dbEntity);
			this.uow.SaveChanges();

			return this.ConvertToDto(dbEntity);
		}

		/// <summary>
		/// Update di un record esistente
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public virtual TDto Update(TDto model)
		{
			TEntity viewEntity = ConvertToEntity(model);
			TEntity dbEntity = this.repository.GetById(viewEntity.Id);

			TEntity entityToSave = UpdateProperties(viewEntity, dbEntity);

			this.repository.Update(entityToSave);
			this.uow.SaveChanges();
			return this.ConvertToDto(dbEntity);
		}

		/// <summary>
		/// Eliminazione di un singolo record ricercato per chiave primaria
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public virtual void Delete(TPrimaryKey key)
		{
			this.repository.Delete(key);
			this.uow.SaveChanges();
		}

		/// <summary>
		/// Conversione da Dto a Entity tramite AutoMapper
		/// </summary>
		/// <param name="dto"></param>
		/// <returns></returns>
		protected virtual TEntity ConvertToEntity(TDto dto)
		{
			return Mapper.Map<TEntity>(dto);
		}

		/// <summary>
		/// Metoto astratto da implementare nella classe derivata del servizio per assegnare le properties da aggiornare durante l'update
		/// </summary>
		/// <param name="viewEntity"></param>
		/// <param name="dbEntity"></param>
		/// <returns></returns>
		protected abstract TEntity UpdateProperties(TEntity viewEntity, TEntity dbEntity);

	}
}
