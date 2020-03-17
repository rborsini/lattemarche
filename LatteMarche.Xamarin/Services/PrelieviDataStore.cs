using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LatteMarche.Xamarin.Services
{
    public class PrelieviDataStore : IDataStore<Prelievo>
    {

        public PrelieviDataStore()
        { }

        public async Task<bool> AddItemAsync(Prelievo item)
        {
            using (var context = CrateContext())
            {
                await context.Prelievi.AddAsync(item);
                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        public async Task<bool> UpdateItemAsync(Prelievo item)
        {
            using (var context = CrateContext())
            {
                var existingPrelievo = context.Prelievi.FirstOrDefaultAsync(p => p.Id == item.Id).Result;

                if(existingPrelievo != null)
                {
                    existingPrelievo.Scomparto = item.Scomparto;
                    existingPrelievo.Quantita = item.Quantita;
                }

                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            using (var context = CrateContext())
            {
                var existingPrelievo = context.Prelievi.FirstOrDefaultAsync(p => p.Id == id).Result;
                context.Remove(existingPrelievo);
                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        public async Task<Prelievo> GetItemAsync(string id)
        {
            using (var context = CrateContext())
            {
                return await context.Prelievi
                                    .AsNoTracking()                                    
                                    .FirstOrDefaultAsync(p => p.Id == id);
            }
        }

        public async Task<IEnumerable<Prelievo>> GetItemsAsync(bool forceRefresh = false)
        {
            using (var context = CrateContext())
            {
                return await context.Prelievi
                                    .AsNoTracking()
                                    .OrderByDescending(escort => escort.DataPrelievo)
                                    .ToListAsync();
            }
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
