using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace LatteMarche.Xamarin.Services
{
    public abstract class BaseDbDataStore<TEntity, TPrimaryKey> : IDataStore<TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
    {
        public async Task<bool> AddRangeItemAsync(IEnumerable<TEntity> items)
        {
            using (var context = CrateContext())
            {
                await context.Set<TEntity>().AddRangeAsync(items);
                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        public async Task<bool> AddItemAsync(TEntity item)
        {
            using (var context = CrateContext())
            {
                await context.Set<TEntity>().AddAsync(item);
                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        public async Task<bool> DeleteItemAsync(TPrimaryKey id)
        {
            using (var context = CrateContext())
            {
                var existingPrelievo = context.Set<TEntity>().FirstOrDefaultAsync(p => p.Id.Equals(id)).Result;
                context.Remove(existingPrelievo);
                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        public async Task<bool> DeleteAllItemsAsync()
        {
            using (var context = CrateContext())
            {
                context.Set<TEntity>().RemoveRange(context.Set<TEntity>());
                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        public virtual async Task<TEntity> GetItemAsync(TPrimaryKey id)
        {
            using (var context = CrateContext())
            {
                return await context.Set<TEntity>()
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(p => p.Id.Equals(id));
            }
        }

        public async Task<IEnumerable<TEntity>> GetItemsAsync()
        {
            using (var context = CrateContext())
            {
                return await context.Set<TEntity>()
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<TEntity>> GetItemsAsync(Func<TEntity, bool> whereFunc)
        {
            using (var context = CrateContext())
            {
                return await context.Set<TEntity>()
                                    .Where(i => whereFunc(i))
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }

        public async Task<bool> UpdateItemAsync(TEntity item)
        {
            using (var context = CrateContext())
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

        protected LatteMarcheDbContext CrateContext()
        {
            LatteMarcheDbContext databaseContext = (LatteMarcheDbContext)Activator.CreateInstance(typeof(LatteMarcheDbContext));
            databaseContext.Database.EnsureCreated();
            databaseContext.Database.Migrate();
            return databaseContext;
        }


    }
}
