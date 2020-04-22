using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Db.Services
{
    public class SincronizzazioneService : ISincronizzazioneService
    {
        public async Task<bool> AddAsync(SynchType tipo)
        {
            using (var context = CrateContext())
            {
                var item = new Sincronizzazione()
                {
                    Timestamp = DateTime.Now,
                    Tipo = tipo.ToString()
                };

                await context.Set<Sincronizzazione>().AddAsync(item);
                await context.SaveChangesAsync();
                return await Task.FromResult(true);
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
