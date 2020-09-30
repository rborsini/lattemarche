using LatteMarche.Xamarin.Db;
using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Db.Services
{
    public class AutoCisterneService : BaseEntityService<AutoCisterna, int>, IAutoCisterneService
    {
        public async Task<AutoCisterna> GetDefaultAsync()
        {
            using (var context = CreateContext())
            {
                if(context.Set<AutoCisterna>().CountAsync(a => a.Selezionata).Result == 1)
                    return await context.Set<AutoCisterna>().FirstOrDefaultAsync(a => a.Selezionata);
                else
                    return await context.Set<AutoCisterna>().FirstOrDefaultAsync();
            }
        }

        public async Task<bool> SetDefaultAsync(int idAutocisterna)
        {
            using (var context = CreateContext())
            {
                foreach (var autocisterna in context.Set<AutoCisterna>())
                {
                    autocisterna.Selezionata = autocisterna.Id == idAutocisterna;
                    context.Update<AutoCisterna>(autocisterna);
                }

                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

    }
}
