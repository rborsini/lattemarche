using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Db.Services
{
    public class StampantiService : IStampantiService
    {

        public async Task<IEnumerable<Stampante>> GetItemsAsync()
        {
            using (var context = CrateContext())
            {
                return await context.Set<Stampante>()
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }


        public async Task<bool> InsertOrUpdateAsync(Stampante stampante)
        {
            using (var context = CrateContext())
            {
                var entity = context.Set<Stampante>().FirstOrDefaultAsync(s => s.MacAddress == stampante.MacAddress).Result;

                if(entity != null)
                {
                    entity.Nome = stampante.Nome;
                    entity.MacAddress = stampante.MacAddress;

                    context.Update<Stampante>(entity);
                }
                else
                {
                    context.Add<Stampante>(stampante);
                }

                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        public async Task<Stampante> GetDefaultAsync()
        {
            using (var context = CrateContext())
            {
                return await context.Set<Stampante>().FirstOrDefaultAsync(p => p.Selezionata);
            }
        }

        public async Task<bool> SetDefaultAsync(string macAddress)
        {
            using (var context = CrateContext())
            {
                foreach(var stampante in context.Set<Stampante>())
                {
                    stampante.Selezionata = stampante.MacAddress == macAddress;
                    context.Update<Stampante>(stampante);                    
                }

                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        protected LatteMarcheDbContext CrateContext()
        {
            LatteMarcheDbContext databaseContext = (LatteMarcheDbContext)Activator.CreateInstance(typeof(LatteMarcheDbContext));
            return databaseContext;
        }

    }
}
