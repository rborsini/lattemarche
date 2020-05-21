using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace LatteMarche.Xamarin.Db
{
    public abstract class BaseEntityService<TEntity, TPrimaryKey> : IEntityService<TEntity, TPrimaryKey>
        where TEntity : AbstractEntity<TPrimaryKey>
    {
        public virtual async Task<bool> AddRangeItemAsync(IEnumerable<TEntity> items)
        {
            using (var context = CreateContext())
            {
                await context.Set<TEntity>().AddRangeAsync(items);
                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        public virtual async Task<bool> AddItemAsync(TEntity item)
        {
            using (var context = CreateContext())
            {
                await context.Set<TEntity>().AddAsync(item);
                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        public virtual async Task<bool> DeleteItemAsync(TPrimaryKey id)
        {
            using (var context = CreateContext())
            {
                var existingPrelievo = context.Set<TEntity>().FirstOrDefaultAsync(p => p.Id.Equals(id)).Result;
                context.Remove(existingPrelievo);
                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        public virtual async Task<bool> DeleteAllItemsAsync()
        {
            using (var context = CreateContext())
            {
                context.Set<TEntity>().RemoveRange(context.Set<TEntity>());
                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        public virtual async Task<TEntity> GetItemAsync(TPrimaryKey id)
        {
            using (var context = CreateContext())
            {
                return await context.Set<TEntity>()
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(p => p.Id.Equals(id));
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetItemsAsync()
        {
            using (var context = CreateContext())
            {
                return await context.Set<TEntity>()
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }

        public virtual async Task<bool> UpdateItemAsync(TEntity item)
        {
            using (var context = CreateContext())
            {
                var existingPrelievo = context.Set<TEntity>().FirstOrDefaultAsync(p => p.Id.Equals(item.Id)).Result;

                if (existingPrelievo != null)
                {
                    existingPrelievo = UpdateProperties(existingPrelievo, item);
                }

                context.Update<TEntity>(existingPrelievo);

                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        protected virtual TEntity UpdateProperties(TEntity entityItem, TEntity viewItem)
        {
            return entityItem;
        }

        protected LatteMarcheDbContext CreateContext()
        {
            LatteMarcheDbContext databaseContext = (LatteMarcheDbContext)Activator.CreateInstance(typeof(LatteMarcheDbContext));
            //databaseContext.Database.EnsureCreated();
            //databaseContext.Database.Migrate();
            return databaseContext;
        }


    }
}
