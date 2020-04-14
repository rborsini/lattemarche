using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Services
{
    public class GiroItemsDataStore : BaseDbDataStore<GiroItem, string>, IGiroItemsService
    {
        public async Task<IEnumerable<GiroItem>> GetItems(int idGiro)
        {
            using (var context = CrateContext())
            {
                return await context.Set<GiroItem>()
                    .Where(i => i.IdGiro == idGiro)
                    .ToListAsync();
            }

        }
    }
}
